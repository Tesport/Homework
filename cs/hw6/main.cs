using System;

// 1
struct Fraction
{
    public int Numerator { get; }
    public int Denominator { get; }

    public Fraction(int numerator, int denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Знаменатель не может быть равен нулю.");

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        int gcd = Gcd(Math.Abs(numerator), denominator);
        Numerator = numerator / gcd;
        Denominator = denominator / gcd;
    }

    private static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int t = a % b;
            a = b;
            b = t;
        }
        return a == 0 ? 1 : a;
    }

    public static Fraction operator &(Fraction a, Fraction b)
    {
        return new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }

    public static Fraction operator |(Fraction a, Fraction b)
    {
        return new Fraction(
            a.Numerator * b.Denominator + b.Numerator * a.Denominator,
            a.Denominator * b.Denominator);
    }

    public static Fraction operator ^(Fraction a, Fraction b)
    {
        return new Fraction(
            a.Numerator * b.Denominator - b.Numerator * a.Denominator,
            a.Denominator * b.Denominator);
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }
}

// 2
[Flags]
enum Day
{
    None = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 4,
    Thursday = 8,
    Friday = 16,
    Saturday = 32,
    Sunday = 64
}

struct Weekdays
{
    public Day Days { get; }

    public Weekdays(Day days)
    {
        Days = days;
    }

    public static Weekdays operator &(Weekdays a, Weekdays b)
    {
        return new Weekdays(a.Days & b.Days);
    }

    public static Weekdays operator |(Weekdays a, Weekdays b)
    {
        return new Weekdays(a.Days | b.Days);
    }

    public static Weekdays operator ^(Weekdays a, Weekdays b)
    {
        return new Weekdays(a.Days ^ b.Days);
    }

    public bool HasFlag(Day day)
    {
        return (Days & day) == day;
    }

    public override string ToString()
    {
        return Days.ToString();
    }
}

// 3
struct EvenNumber
{
    public int Value { get; }

    public EvenNumber(int value)
    {
        Value = value;
    }

    private bool IsEven => Value % 2 == 0;

    public static bool operator &(EvenNumber a, EvenNumber b)
    {
        return a.IsEven && b.IsEven;
    }

    public static bool operator |(EvenNumber a, EvenNumber b)
    {
        return a.IsEven || b.IsEven;
    }

    public static bool operator ^(EvenNumber a, EvenNumber b)
    {
        return a.IsEven != b.IsEven;
    }
}

// 4
struct TimeInterval
{
    public int Hours { get; }
    public int Minutes { get; }

    public TimeInterval(int hours, int minutes)
    {
        int total = hours * 60 + minutes;
        if (total < 0)
            throw new ArgumentException("Интервал не может быть отрицательным.");

        Hours = total / 60;
        Minutes = total % 60;
    }

    public int TotalMinutes => Hours * 60 + Minutes;

    public static TimeInterval operator &(TimeInterval a, TimeInterval b)
    {
        return a.TotalMinutes <= b.TotalMinutes ? a : b;
    }

    public static TimeInterval operator |(TimeInterval a, TimeInterval b)
    {
        return a.TotalMinutes >= b.TotalMinutes ? a : b;
    }

    public static int operator ^(TimeInterval a, TimeInterval b)
    {
        return Math.Abs(a.TotalMinutes - b.TotalMinutes);
    }

    public override string ToString()
    {
        return $"{Hours} ч {Minutes} мин";
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Задание 1");
        var f1 = new Fraction(1, 2);
        var f2 = new Fraction(1, 3);
        Console.WriteLine($"{f1} & {f2} = {f1 & f2}");
        Console.WriteLine($"{f1} | {f2} = {f1 | f2}");
        Console.WriteLine($"{f1} ^ {f2} = {f1 ^ f2}");

        Console.WriteLine("\nЗадание 2");
        var w1 = new Weekdays(Day.Monday | Day.Tuesday | Day.Friday);
        var w2 = new Weekdays(Day.Tuesday | Day.Wednesday | Day.Friday);
        Console.WriteLine($"w1: {w1}");
        Console.WriteLine($"w2: {w2}");
        Console.WriteLine($"w1 & w2: {w1 & w2}");
        Console.WriteLine($"w1 | w2: {w1 | w2}");
        Console.WriteLine($"w1 ^ w2: {w1 ^ w2}");
        Console.WriteLine($"w1 содержит Monday: {w1.HasFlag(Day.Monday)}");
        Console.WriteLine($"w1 содержит Sunday: {w1.HasFlag(Day.Sunday)}");

        Console.WriteLine("\nЗадание 3");
        var e1 = new EvenNumber(4);
        var e2 = new EvenNumber(7);
        var e3 = new EvenNumber(10);
        Console.WriteLine($"4 & 10: {e1 & e3}");
        Console.WriteLine($"4 & 7: {e1 & e2}");
        Console.WriteLine($"4 | 7: {e1 | e2}");
        Console.WriteLine($"4 ^ 7: {e1 ^ e2}");
        Console.WriteLine($"4 ^ 10: {e1 ^ e3}");

        Console.WriteLine("\nЗадание 4");
        var t1 = new TimeInterval(1, 30);
        var t2 = new TimeInterval(0, 75);
        Console.WriteLine($"t1: {t1}");
        Console.WriteLine($"t2: {t2}");
        Console.WriteLine($"t1 & t2 (минимальный): {t1 & t2}");
        Console.WriteLine($"t1 | t2 (максимальный): {t1 | t2}");
        Console.WriteLine($"t1 ^ t2 (разность): {t1 ^ t2} мин");
    }
}