using System;
using System.Threading;

class Program
{
    private static SemaphoreSlim[] forks = new SemaphoreSlim[5]; 
    private static SemaphoreSlim table = new SemaphoreSlim(4, 4); 
    static void Main(string[] args)
    {
        for (int i = 0; i < 5; i++)
        {
            forks[i] = new SemaphoreSlim(1, 1); 
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

        table.Wait();

        forks[leftFork].Wait(); 
        Console.WriteLine($"Filozof {id} podniósł lewy widelec {leftFork}.");

        forks[rightFork].Wait(); 
        Console.WriteLine($"Filozof {id} podniósł prawy widelec {rightFork}.");

        Console.WriteLine($"Filozof {id} je.");
        Thread.Sleep(1000); 

        forks[rightFork].Release(); 
        Console.WriteLine($"Filozof {id} odłożył prawy widelec {rightFork}.");

        forks[leftFork].Release();
        Console.WriteLine($"Filozof {id} odłożył lewy widelec {leftFork}.");

        table.Release();
    }
}