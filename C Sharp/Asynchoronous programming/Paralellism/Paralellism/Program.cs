using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Paralellism
{
    class Program
    {
        static ThreadLocal<decimal?> threadLocal = new ThreadLocal<decimal?>();
        static void Main(string[] args)
        {
            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 2
            };

            Parallel.For(0, 100, options, (i) =>
            {
                var currentValue = threadLocal.Value;
                threadLocal.Value = Compute(i);

            });
            /*var stopwatch = new Stopwatch();
            stopwatch.Start();

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(2000);

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cancellationTokenSource.Token,
                MaxDegreeOfParallelism = 1
            };

            int total = 0;
            try
            {
                Parallel.For(0, 100, parallelOptions, (i) =>
                {
                    var result = Compute(i);
                    Interlocked.Add(ref total, (int)result);

                });
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            Console.WriteLine(total);

            Console.WriteLine($"It took {stopwatch.ElapsedMilliseconds}ms to run");*/
        }

        static Random random = new Random();
        static decimal Compute(int value)
        {
            var randomMilliseconds = random.Next(10, 50);
            var end = DateTime.Now + TimeSpan.FromMilliseconds(randomMilliseconds);
            while(DateTime.Now < end)
            {

            }
            return value + 0.5m;
        }
    }
}
