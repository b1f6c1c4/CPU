using System;
using System.Threading;

namespace AssemblerGui
{
    public class AsmAsyncDebugger : IDisposable
    {
        private enum Executing
        {
            None,
            Instruction,
            Statement,
            Procedure,
            Frame,
            All,
            Quit
        }

        public event SimpleEventHandler OnPause;

        public event SimpleEventHandler OnExited;

        public event SimpleEventHandler OnStarted;

        private readonly AsmDebugger m_Debugger;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Thread m_Worker;

        private bool m_Disposed;

        private readonly object m_Lock = new object();

        private Executing m_Request;

        private CancellationTokenSource m_Source;

        private CancellationToken m_Token;

        public AsmAsyncDebugger(AsmDebugger debugger)
        {
            m_Debugger = debugger;
            m_Debugger.OnPause += () => OnPause?.Invoke();
            m_Worker = new Thread(WorkerThreadEntryPoint) { Name = "Worker", IsBackground = true };
            m_Worker.Start();
        }

        private void RequestRunning(Executing request)
        {
            lock (m_Lock)
            {
                m_Source = new CancellationTokenSource();
                m_Token = m_Source.Token;
                m_Request = request;
                if (request != Executing.Quit)
                    OnStarted?.Invoke();
                Monitor.Pulse(m_Lock);
            }
        }

        public void Run() => RequestRunning(Executing.All);

        public void Pause() => m_Source?.Cancel();

        public void Stop()
        {
            m_Source?.Cancel();
            RequestRunning(Executing.Quit);
        }

        public void NextInstruction() => RequestRunning(Executing.Instruction);

        public void NextStatement() => RequestRunning(Executing.Statement);

        public void NextProcedure() => RequestRunning(Executing.Procedure);

        public void JumpOut() => RequestRunning(Executing.Frame);

        private void WorkerThreadEntryPoint()
        {
            while (true)
            {
                Executing request;
                lock (m_Lock)
                {
                    Monitor.Wait(m_Lock);
                    request = m_Request;
                }

                try
                {
                    switch (request)
                    {
                        case Executing.None:
                            continue;
                        case Executing.Quit:
                            return;
                        case Executing.Instruction:
                            m_Debugger.NextInstruction();
                            break;
                        case Executing.Statement:
                            m_Debugger.NextStatement();
                            break;
                        case Executing.Procedure:
                            m_Debugger.NextProcedure(m_Token);
                            break;
                        case Executing.Frame:
                            m_Debugger.JumpOut(m_Token);
                            break;
                        case Executing.All:
                            m_Debugger.Run(m_Token);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (HaltException)
                {
                    OnExited?.Invoke();
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
