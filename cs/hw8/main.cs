using System;
using System.Collections.Generic;

class Program
{
    // 1
    static int[,] Multiply(int[,] a, int[,] b)
    {
        int k = a.GetLength(0);
        int m = a.GetLength(1);
        int n = b.GetLength(1);

        if (m != b.GetLength(0))
            throw new ArgumentException("Число столбцов A должно быть равно числу строк B.");

        int[,] c = new int[k, n];
        for (int i = 0; i < k; i++)
            for (int j = 0; j < n; j++)
                for (int t = 0; t < m; t++)
                    c[i, j] += a[i, t] * b[t, j];

        return c;
    }

    // 2
    static void RotateClockwise(int[,] matrix)
    {
        int n = matrix.GetLength(0);
        if (n != matrix.GetLength(1))
            throw new ArgumentException("Матрица должна быть квадратной.");

        for (int layer = 0; layer < n / 2; layer++)
        {
            int first = layer;
            int last = n - 1 - layer;
            for (int i = first; i < last; i++)
            {
                int offset = i - first;
                int top = matrix[first, i];

                matrix[first, i] = matrix[last - offset, first];
                matrix[last - offset, first] = matrix[last, last - offset];
                matrix[last, last - offset] = matrix[i, last];
                matrix[i, last] = top;
            }
        }
    }

    // 3
    static List<(int Row, int Col, int Value)> FindSaddlePoints(int[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        var result = new List<(int, int, int)>();

        for (int i = 0; i < rows; i++)
        {
            int min = matrix[i, 0];
            for (int j = 1; j < cols; j++)
                if (matrix[i, j] < min)
                    min = matrix[i, j];

            for (int j = 0; j < cols; j++)
            {
                if (matrix[i, j] != min)
                    continue;

                bool isMaxInColumn = true;
                for (int r = 0; r < rows; r++)
                {
                    if (matrix[r, j] > min)
                    {
                        isMaxInColumn = false;
                        break;
                    }
                }

                if (isMaxInColumn)
                    result.Add((i, j, min));
            }
        }

        return result;
    }

    // 4
    static List<int> SpiralOrder(int[,] matrix)
    {
        var result = new List<int>();
        int top = 0;
        int bottom = matrix.GetLength(0) - 1;
        int left = 0;
        int right = matrix.GetLength(1) - 1;

        while (top <= bottom && left <= right)
        {
            for (int j = left; j <= right; j++)
                result.Add(matrix[top, j]);
            top++;

            for (int i = top; i <= bottom; i++)
                result.Add(matrix[i, right]);
            right--;

            if (top <= bottom)
            {
                for (int j = right; j >= left; j--)
                    result.Add(matrix[bottom, j]);
                bottom--;
            }

            if (left <= right)
            {
                for (int i = bottom; i >= top; i--)
                    result.Add(matrix[i, left]);
                left++;
            }
        }

        return result;
    }

    // 5
    static int[,] FillSnake(int n)
    {
        int[,] matrix = new int[n, n];
        int value = 1;

        for (int i = 0; i < n; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < n; j++)
                    matrix[i, j] = value++;
            }
            else
            {
                for (int j = n - 1; j >= 0; j--)
                    matrix[i, j] = value++;
            }
        }

        return matrix;
    }

    static void Print(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
                Console.Write($"{matrix[i, j],4}");
            Console.WriteLine();
        }
    }

    static void Main()
    {
        Console.WriteLine("Задание 1: умножение матриц");
        int[,] a = { { 1, 2, 3 }, { 4, 5, 6 } };
        int[,] b = { { 7, 8 }, { 9, 10 }, { 11, 12 } };
        Print(Multiply(a, b));

        Console.WriteLine("\nЗадание 2: поворот на 90 градусов");
        int[,] square = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
        RotateClockwise(square);
        Print(square);

        Console.WriteLine("\nЗадание 3: седловая точка");
        int[,] saddle = { { 3, 5, 4 }, { 1, 2, 0 }, { 7, 8, 6 } };
        var points = FindSaddlePoints(saddle);
        if (points.Count == 0)
            Console.WriteLine("Седловых точек нет.");
        foreach (var p in points)
            Console.WriteLine($"Координаты: ({p.Row}, {p.Col}), значение: {p.Value}");

        Console.WriteLine("\nЗадание 4: спиральный обход");
        int[,] spiral = { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } };
        Console.WriteLine(string.Join(" ", SpiralOrder(spiral)));

        Console.WriteLine("\nЗадание 5: заполнение змейкой");
        Print(FillSnake(5));
    }
}