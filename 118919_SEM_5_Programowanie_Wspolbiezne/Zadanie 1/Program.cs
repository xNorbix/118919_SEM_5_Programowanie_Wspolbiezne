
using System;
using System.Threading;

class Program
{
    static int counter = 0;
    static object lockObject = new object();

    static void IncrementCounter()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId; 
        Console.WriteLine($"[Thread {threadId}] Start pracy");

        for (int i = 0; i < 10_000_000; i++)
        {
            lock (lockObject) 
            {
                counter++;
            }
        }

        Console.WriteLine($"[Thread {threadId}] Zakończono pracę");
    }

    static void Main()
    {
        Thread[] threads = new Thread[10];

        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(IncrementCounter);
            threads[i].Start();
        }

        // Czekamy na zakończenie wszystkich wątków
        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine($"Ostateczna wartość licznika: {counter}");
        Console.ReadKey();
    }
}

