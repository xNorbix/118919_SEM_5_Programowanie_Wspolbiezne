using System;
using System.Threading;

class ReadersWritersProblem
{
    private static SemaphoreSlim readLock = new SemaphoreSlim(1, 1);
    private static SemaphoreSlim writeLock = new SemaphoreSlim(1, 1);
    private static int readCount = 0;
    private static int sharedResource = 0;

    static void Main(string[] args)
    {
        Thread[] readers = new Thread[5];
        Thread[] writers = new Thread[2];

        for (int i = 0; i < readers.Length; i++)
        {
            readers[i] = new Thread(Reader);
            readers[i].Start();
        }

        for (int i = 0; i < writers.Length; i++)
        {
            writers[i] = new Thread(Writer);
            writers[i].Start();
        }

        foreach (var reader in readers)
        {
            reader.Join();
        }

        foreach (var writer in writers)
        {
            writer.Join();
        }
    }

    static void Reader()
    {
        while (true)
        {
            readLock.Wait();
            readCount++;
            if (readCount == 1)
            {
                writeLock.Wait();
            }
            readLock.Release();
            Console.WriteLine(readCount);
            Console.WriteLine($"Czytelnik {Thread.CurrentThread.ManagedThreadId} czyta wartość: {sharedResource}");
            Thread.Sleep(1000);
            readLock.Wait();
            readCount--;
            if (readCount == 0)
            {
                writeLock.Release();
            }
            readLock.Release(); 
            Thread.Sleep(1000); 
        }
    }

    static void Writer()
    {
        while (true)
        {
            writeLock.Wait();
            Random rand = new Random();
            sharedResource = rand.Next(1,9999);
            Console.WriteLine($"Pisarz {Thread.CurrentThread.ManagedThreadId} pisze wartość: {sharedResource}");
            Thread.Sleep(1000); 
            writeLock.Release();
            Thread.Sleep(1000); 
        }
    }
}