using System;
using System.Threading;

class Program
{
    private static readonly object[] forks = new object[5]; 
    private static readonly object tableLock = new object(); 
    private static int philosophersAtTable = 0;

    static void Main(string[] args)
    {
        for (int i = 0; i < 5; i++)
        {
            forks[i] = new object();
        }

        Thread[] philosophers = new Thread[5];
        for (int i = 0; i < 5; i++)
        {
            int id = i;
            philosophers[i] = new Thread(() => Philosopher(id));
            philosophers[i].Start();
        }

        foreach (var philosopher in philosophers)
        {
            philosopher.Join();
        }
    }

    static void Philosopher(int id)
    {
        int leftFork = id;
        int rightFork = (id + 1) % 5;

        Console.WriteLine($"Filozof {id} myśli.");
        Thread.Sleep(1000); 

        Console.WriteLine($"Filozof {id} jest głodny i próbuje jeść.");

        lock (tableLock)
        {
            while (philosophersAtTable >= 4)
            {
                Monitor.Wait(tableLock); 
            }
            philosophersAtTable++;
        }

        lock (forks[leftFork])
        {
            lock (forks[rightFork])
            {
                Console.WriteLine($"Filozof {id} podniósł lewy widelec {leftFork}.");
                Console.WriteLine($"Filozof {id} podniósł prawy widelec {rightFork}.");

                Console.WriteLine($"Filozof {id} je.");
                Thread.Sleep(1000); 

                Console.WriteLine($"Filozof {id} odłożył prawy widelec {rightFork}.");
                Console.WriteLine($"Filozof {id} odłożył lewy widelec {leftFork}.");
            }
        }

        lock (tableLock)
        {
            philosophersAtTable--; 
            Monitor.Pulse(tableLock); 
        }
    }
}