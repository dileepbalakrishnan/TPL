using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TaskCompletionSourceDemo
{
    public class TextProcessor
    {
        private StreamReader _reader;
        private readonly ConcurrentDictionary<string, int> _words = new ConcurrentDictionary<string, int>();

        private readonly TaskCompletionSource<IDictionary<string, int>> _processorCompletionSource =
            new TaskCompletionSource<IDictionary<string, int>>();

        public TextProcessor(string fileName)
        {
            var fileNme = fileName ?? throw new ArgumentException(nameof(fileName));
            Task.Factory.StartNew(() =>
            {
                try
                {
                    _reader = new StreamReader(fileNme);
                    ReadLine();
                }
                catch (Exception ex)
                {
                    _processorCompletionSource.SetException(ex);
                }
            });
        }

        public Task<IDictionary<string, int>> WordCountTask => _processorCompletionSource.Task;

        private void ReadLine()
        {
            _reader.ReadLineAsync().ContinueWith(ProcessLine);
        }

        private void ProcessLine(Task<string> t)
        {
            if (t.IsFaulted)
            {
                _processorCompletionSource.SetException(t.Exception.InnerExceptions);
                _reader.Close();
            }
            else
            {
                var line = t.Result;
                if (line != null)
                {
                    foreach (var word in line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries))
                        _words.AddOrUpdate(word, 1, (key, count) => count + 1);
                    ReadLine();
                }
                else
                {
                    _reader.Close();
                    _processorCompletionSource.SetResult(_words);
                }
            }
        }
    }
}