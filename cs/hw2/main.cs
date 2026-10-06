using System;

public static class Utils
{
    // 1
    public static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }

    // 2
    public static T FindMax<T>(T[] array) where T : IComparable<T>
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (array.Length == 0)
            throw new ArgumentException("Durak ti", nameof(array));

        T max = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i].CompareTo(max) > 0)
                max = array[i];
        }
        return max;
    }
}

// 3
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, null);

    public static Result<T> Failure(string error) => new Result<T>(false, default, error);

    public T GetValueOrElse(T defaultValue) => IsSuccess ? Value : defaultValue;
}

public static class Program
{
    public static void Main()
    {
        // 1
        int x = 5, y = 10;
        Utils.Swap(ref x, ref y);
        Console.WriteLine($"x={x}, y={y}");

        string s1 = "a", s2 = "b";
        Utils.Swap(ref s1, ref s2);
        Console.WriteLine($"s1={s1}, s2={s2}");

        // 2
        Console.WriteLine(Utils.FindMax(new[] { 3, 9, 2, 7 }));
        Console.WriteLine(Utils.FindMax(new[] { "shveps", "sueta", "chupepe" }));

        // 3
        var ok = Result<int>.Success(42);
        var fail = Result<int>.Failure("duraley");

        Console.WriteLine(ok.GetValueOrElse(-1));
        Console.WriteLine(fail.GetValueOrElse(-1));
        Console.WriteLine(fail.Error);
    }
}