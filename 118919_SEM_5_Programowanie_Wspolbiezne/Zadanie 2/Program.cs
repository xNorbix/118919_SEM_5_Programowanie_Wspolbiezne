using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static int threadCount = 0; // Licznik utworzonych wątków
    static int maxThreads = 6; // Ograniczenie do liczby rdzeni

    static int Partition(int[] array, int left, int right)
    {
        int pivot = array[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (array[j] <= pivot)
            {
                i++;
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
        (array[i + 1], array[right]) = (array[right], array[i + 1]);
        return i + 1;
    }

    static void SequentialQuickSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(array, left, right);
            SequentialQuickSort(array, left, pivot - 1);
            SequentialQuickSort(array, pivot + 1, right);
        }
    }

    static async Task ParallelQuickSortAsync(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(array, left, right);

            if (Interlocked.Increment(ref threadCount) < maxThreads)
            {
                Task leftTask = Task.Run(() => ParallelQuickSortAsync(array, left, pivot - 1));
                Task rightTask = Task.Run(() => ParallelQuickSortAsync(array, pivot + 1, right));

                await Task.WhenAll(leftTask, rightTask);
            }
            else
            {
                SequentialQuickSort(array, left, pivot - 1);
                SequentialQuickSort(array, pivot + 1, right);
            }
        }
    }

    static int[] GenerateRandomArray(int size)
    {
        Random rand = new Random();
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = rand.Next(0, size);
        }
        return array;
    }

    static bool IsSorted(int[] array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < array[i - 1])
                return false;
        }
        return true;
    }

    static async Task Main()
    {
        int count = 10_000_000;

        int[] array1 = GenerateRandomArray(count);

        var start = DateTime.Now;
        await ParallelQuickSortAsync(array1, 0, array1.Length - 1);
        var end = DateTime.Now;

        Console.WriteLine($"Równoległy QuickSort ({count} elementów): {end - start}");
        Console.WriteLine($"Czy tablica jest posortowana? {IsSorted(array1)}");
        Console.WriteLine($"Liczba utworzonych wątków: {threadCount}");

        threadCount = 0;

        int[] array2 = GenerateRandomArray(count);

        start = DateTime.Now;
        SequentialQuickSort(array2, 0, array2.Length - 1);
        end = DateTime.Now;

        Console.WriteLine($"Sekwencyjny QuickSort ({count} elementów): {end - start}");
        Console.WriteLine($"Czy tablica jest posortowana? {IsSorted(array2)}");
    }
}
