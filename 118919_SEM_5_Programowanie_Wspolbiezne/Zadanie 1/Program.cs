/*
 * Zadanie 1. Napisz program komputerowy,
 * który powołuje do życia 5-10 wątków działających na współdzielonej pamięci
 * (współdzielonych zmiennych). Zaobserwuj działanie programu w kolejnych jego
 * uruchomieniach. Co możesz powiedzieć?
*/
using System;
using System.Threading;

class Program
{
    static int counter = 0;
    static object lockObject = new object();

    static void IncrementCounter()
    {
        int threadId = Thread.CurrentThread.ManagedThreadId; // Pobieramy ID wątku
        Console.WriteLine($"[Thread {threadId}] Start pracy");

        for (int i = 0; i < 10_000_000; i++)
        {
            lock (lockObject) // Blokada aby jeden wątek na raz miał dostęp do zmiennej counter 
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

