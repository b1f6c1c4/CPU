using System;
using System.Threading;

namespace AssemblerGui
{
    public delegate void ExceptionEventHandler(Exception e);

    public class AsmAsyncDebugger : IDisposable
    {
        public event SimpleEventHandler OnPause;

        public event SimpleEventHandler OnExited;

        public event SimpleEventHandler OnStarted;

        public event ExceptionEventHandler OnError;

        private readonly AsmDebugger m_Debugger;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Thread m_Worker;

        private bool m_Disposed;

        private readonly object m_Lock = new object();

        private PauseCriterion m_Request;

        private bool m_RequestQuit;

        private CancellationTokenSource m_Source;

        private CancellationToken m_Token;

        public AsmAsyncDebugger(AsmDebugger debugger)
        {
            m_Debugger = debugger;
            m_Debugger.OnPause += () => OnPause?.Invoke();
            m_Worker = new Thread(WorkerThreadEntryPoint) { Name = "Worker", IsBackground = true };
            m_Worker.Start();
        }

        private void RequestQuit()
        {
            lock (m_Lock)
            {
                m_Source = new CancellationTokenSource();
                m_Token = m_Source.Token;
                m_Request = null;
                m_RequestQuit = true;
                Monitor.Pulse(m_Lock);
            }
        }

        private void RequestRunning(PauseCriterion request)
        {
            lock (m_Lock)
            {
                m_Source = new CancellationTokenSource();
                m_Token = m_Source.Token;
                m_Request = new Cancellable(request, m_Token);
                m_RequestQuit = false;
                OnStarted?.Invoke();
                Monitor.Pulse(m_Lock);
            }
        }

        public void Run() => RequestRunning(null);

        public void Pause() => m_Source?.Cancel();

        public void Stop()
        {
            m_Source?.Cancel();
            RequestQuit();
        }

        private void Next(int lease, PauseCriterion criterion)
        {
            try
            {
                m_Debugger.Next(ref lease, criterion);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e);
                return;
            }
            if (lease == 0)
                RequestRunning(criterion);
        }

        public void NextInstruction(int lease = 0) => Next(lease, new NextInstruction());

        public void NextStatement(int lease = 0) => Next(lease, new NextStatement(m_Debugger.Source));

        public void NextProcedure(int lease = 0) => Next(lease, new NextProcedure(m_Debugger.Source));

        public void JumpOut(int lease = 0) => Next(lease, new JumpOut(m_Debugger.CPU, m_Debugger.Source));

        private void WorkerThreadEntryPoint()
        {
            while (true)
            {
                PauseCriterion request;
                lock (m_Lock)
                {
                    Monitor.Wait(m_Lock);
                    if (m_RequestQuit)
                        return;
                    request = m_Request;
                }

                try
                {
                    m_Debugger.Next(request);
                }
                catch (HaltException)
                {
                    OnExited?.Invoke();
                    break;
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                    break;
                }
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_Disposed)
                return;

            if (disposing)
                Stop();

            m_Disposed = true;
        }

        ~AsmAsyncDebugger() { Dispose(false); }
    }
}
