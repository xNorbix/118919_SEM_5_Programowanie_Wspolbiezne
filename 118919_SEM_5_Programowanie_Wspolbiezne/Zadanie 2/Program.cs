using System;
using System.Diagnostics;

class Program
{
    public static void Main()
    {
        int count = 100_0;

        int[] array1 = GenerateRandomArray(count);
        var start = DateTime.Now;
        ParallelQuickSort(array1, 0, array1.Length - 1);
        var end = DateTime.Now;
        Console.WriteLine($"Równoległy QuickSort ({count} elementów): {end - start}");
        Console.WriteLine($"Czy tablica jest posortowana? {IsSorted(array1)}");

        int[] array2 = GenerateRandomArray(count);
        start = DateTime.Now;
        QuickSort(array2, 0, array2.Length - 1);
        end = DateTime.Now;
        Console.WriteLine($"Sekwencyjny QuickSort ({count} elementów): {end - start}");
        Console.WriteLine($"Czy tablica jest posortowana? {IsSorted(array2)}");
    }

    async static void ParallelQuickSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(array, left, right);

            Thread leftThread = new Thread(() => ParallelQuickSort(array, left, pivot - 1));
            Thread rightThread = new Thread(() => ParallelQuickSort(array, pivot + 1, right));

            leftThread.Start();
            rightThread.Start();

            leftThread.Join();
            rightThread.Join();
        }
    }

    static int[] GenerateRandomArray(int size)
    {
        Random rand = new Random();
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = rand.Next(0, 1000);
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
    static int Partition(int[] array, int left, int right)
    {
        int pivot = array[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (array[j] <= pivot)
            {
                i++;
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
        int temp1 = array[i + 1];
        array[i + 1] = array[right];
        array[right] = temp1;
        return i + 1;
    }

    static void QuickSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(array, left, right);
            QuickSort(array, left, pivot - 1);  
            QuickSort(array, pivot + 1, right);
        }
    }
}