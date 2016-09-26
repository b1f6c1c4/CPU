using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    public class Preprocessor : IReadOnlyList<string>
    {
        private readonly List<string> m_Files;

        public Preprocessor() { m_Files = new List<string>(); }

        public void Add(string seed)
        {
            var queue = new Queue<string>();
            m_Files.Remove(seed);
            queue.Enqueue(Path.GetFullPath(seed).Replace(@"/", @"\"));

            ResolveDependency(queue);
        }

        public void AddRange(IReadOnlyList<string> seeds)
        {
            var queue = new Queue<string>();
            foreach (var seed in seeds)
                m_Files.Remove(seed);
            foreach (var seed in seeds)
                queue.Enqueue(Path.GetFullPath(seed).Replace(@"/", @"\"));

            ResolveDependency(queue);
        }

        private void ResolveDependency(Queue<string> queue)
        {
            while (queue.Count > 0)
            {
                var fn = queue.Peek();
                queue.Dequeue();

                if (m_Files.Contains(fn))
                    continue;

                m_Files.Add(fn);

                using (var s = new StreamReader(fn))
                    foreach (var r in GetRequirements(s))
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        var rn = Path.Combine(Path.GetDirectoryName(fn), r).Replace(@"/", @"\");
                        if (m_Files.Contains(rn))
                            continue;
                        queue.Enqueue(rn);
                    }
            }
        }

        private static IEnumerable<string> GetRequirements(TextReader fin)
        {
            string s;
            while ((s = fin.ReadLine()) != null)
                if (s.TrimStart().StartsWith(";include", StringComparison.Ordinal))
                    yield return s.TrimStart().Substring(8).Trim();
        }

        public IEnumerator<string> GetEnumerator() => m_Files.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => m_Files.Count;

        public string this[int index] => m_Files[index];
    }
}
