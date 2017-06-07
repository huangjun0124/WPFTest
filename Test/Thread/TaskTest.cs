using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class TaskTest
    {
        public static void T()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<int> t = new Task<int>(() => Add(cts.Token), cts.Token);
            t.Start();
            t.ContinueWith(TaskEnded);
            Console.ReadKey();
            cts.Cancel();
        }

        private static int Add(CancellationToken ct)
        {
            Console.WriteLine("Task begin...");
            int result = 0;
            while (!ct.IsCancellationRequested)
            {
                result++;
                Thread.Sleep(1000);
            }
            return result;
        }

        private static void TaskEnded(Task<int> task)
        {
            Console.WriteLine("Task finished, status is " + Environment.NewLine + "IsCanceled ="+task.IsCanceled
                +@"\tIsCompleted="+task.IsCompleted+@"\tIsFaulted="+task.IsFaulted);
            Console.WriteLine("Task return value is :" + task.Result);
        }

        public static void T2()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<int> t = new Task<int>(() => AddCancelByThrow(cts.Token), cts.Token);
            t.Start();
            t.ContinueWith(TaskEndedByWatch);
            Console.ReadKey();
            cts.Cancel();
        }

        private static int AddCancelByThrow(CancellationToken ct)
        {
            Console.WriteLine("Task begin...");
            int result = 0;
            while (true)
            {
                ct.ThrowIfCancellationRequested();
                result++;
                Thread.Sleep(1000);
            }
            return result;
        }

        private static void TaskEndedByWatch(Task<int> task)
        {
            Console.WriteLine("Task finished, status is " + Environment.NewLine + "IsCanceled =" + task.IsCanceled
                + @"\tIsCompleted=" + task.IsCompleted + @"\tIsFaulted=" + task.IsFaulted);
            try
            {
                Console.WriteLine("Task return value is :" + task.Result);
            }
            catch (AggregateException e)
            {
                e.Handle((err)=>err is OperationCanceledException);
            }
            
        }

        public static void T3()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            TaskFactory factory = new TaskFactory();
            Task[] tasks = new Task[]
            {
                factory.StartNew(()=>Add(cts.Token)),
                factory.StartNew(()=>Add(cts.Token)),
                factory.StartNew(()=>Add(cts.Token))
            };
            factory.ContinueWhenAll(tasks, TasksEnded, CancellationToken.None);
            Console.ReadKey();
            cts.Cancel();
        }

        private static void TasksEnded(Task[] tasks)
        {
            Console.WriteLine("All tasks are finished....");
        }

        public static void ParallelT()
        {
            int[] nums = {1, 2, 3, 4};
            int total = 0;
            Parallel.For<int>(0, nums.Length, () =>
            {
                return 1;
            },
                (i, loopState, subtotal) =>
                {
                    subtotal += nums[i];
                    return subtotal;
                },
                (x) => Interlocked.Add(ref total, x));
            Console.WriteLine("total={0}",total);
        }

        public static void ParallelT1()
        {
            string[] stringArr = new[] {"aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh"};
            string result = string.Empty;
            Parallel.For<string>(0, stringArr.Length, () => "-", (i, loopState, subResult) =>
            {
                return subResult += stringArr[i];
            }, (threadEndString) =>
            {
                result += threadEndString;
                Console.WriteLine("Inner:" + threadEndString);
            });
            Console.WriteLine(result);
        }

        public static void ParallelException()
        {
            try
            {
                var parallelExceptions = new ConcurrentQueue<Exception>();
                Parallel.For(0, 1, (i) =>
                {
                    try
                    {
                        throw new InvalidOperationException("Unknown exception in cussss");
                    }
                    catch (Exception e)
                    {
                        parallelExceptions.Enqueue(e);
                    }
                    if (parallelExceptions.Count > 0)
                    {
                        throw new AggregateException(parallelExceptions);
                    }
                });
            }
            catch (AggregateException err)
            {
                foreach (var item in err.InnerExceptions)
                {
                    Util.Print("Exc type: {0}{1}from: {2}{3}content: {4}", item.InnerException.GetType(), Util.NewLine, item.InnerException.Source, Util.NewLine, item.InnerException.Message);
                }
            }
            Util.Print("Main Thread Exit...");
        }
    }

    class TaskException
    {
        public static void T()
        {
            Task t = new Task(() =>
            {
                throw new InvalidOperationException("Unknown exception in cussss");
            });
            t.Start();
            Task tEnd = t.ContinueWith((task) =>
            {
                throw task.Exception;
            }, TaskContinuationOptions.OnlyOnFaulted);
            try
            {
                tEnd.Wait();
            }
            catch (AggregateException err)
            {
                foreach (var item in err.InnerExceptions)
                {
                    Util.Print("Exc type: {0}{1}from: {2}{3}content: {4}", item.InnerException.GetType(), Util.NewLine, item.InnerException.Source, Util.NewLine,item.InnerException.Message);
                }
            }
            Util.Print("Main Thread Exit...");
        }




        private static event EventHandler<AggregateExceptionArgs> AggregateExceptionCatched;
        public static void T2()
        {
            AggregateExceptionCatched += new EventHandler<AggregateExceptionArgs>(AExceptionCatched);
            Task t = new Task(() =>
            {
                try
                {
                    throw new InvalidOperationException("Unknown exception in cussss");
                }
                catch (Exception err)
                {
                    AggregateExceptionArgs arg = new AggregateExceptionArgs(){AggregateException = new AggregateException(err)};
                    AggregateExceptionCatched(null, arg);
                }
            });
            t.Start();
            Util.Print("Main Thread Exit right away...");
        }

        private static void AExceptionCatched(object sender, AggregateExceptionArgs e)
        {
            foreach (var item in e.AggregateException.InnerExceptions)
            {
                Util.Print("Exc type: {0}{1}from: {2}{3}content: {4}", item.GetType(), Util.NewLine, item.Source, Util.NewLine, item.Message);
            }
        }

        public class AggregateExceptionArgs : EventArgs
        {
            public AggregateException AggregateException { get; set; }
        }
    }

}
