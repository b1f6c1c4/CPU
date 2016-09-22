using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    public class Preprocessor : IEnumerable<string>
    {
        private readonly List<string> m_Files;

        public Preprocessor(IEnumerable<string> seeds)
        {
            m_Files = new List<string>();

            var queue = new Queue<string>();
            foreach (var seed in seeds)
                queue.Enqueue(Path.GetFullPath(seed).Replace(@"/", @"\"));

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
                if (s.StartsWith(";include", StringComparison.Ordinal))
                    yield return s.Substring(8).Trim();
        }

        public IEnumerator<string> GetEnumerator() => m_Files.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
