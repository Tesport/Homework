using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

// 1
struct RomanNumeral
{
    private static readonly (int Value, string Symbol)[] Map =
    {
        (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
        (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
        (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I")
    };

    public int Value { get; }

    public RomanNumeral(int value)
    {
        if (value < 1 || value > 3999)
            throw new ArgumentOutOfRangeException(nameof(value), "Допустимый диапазон: 1-3999.");
        Value = value;
    }

    private static string ToRoman(int number)
    {
        var sb = new StringBuilder();
        foreach (var (value, symbol) in Map)
        {
            while (number >= value)
            {
                sb.Append(symbol);
                number -= value;
            }
        }
        return sb.ToString();
    }

    private static int FromRoman(string text)
    {
        string s = text.Trim().ToUpperInvariant();
        if (s.Length == 0)
            throw new FormatException("Пустая строка.");

        int total = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int current = SymbolValue(s[i]);
            int next = i + 1 < s.Length ? SymbolValue(s[i + 1]) : 0;
            total += current < next ? -current : current;
        }

        if (total < 1 || total > 3999 || ToRoman(total) != s)
            throw new FormatException($"Некорректное римское число: {text}");

        return total;
    }

    private static int SymbolValue(char c)
    {
        switch (c)
        {
            case 'I': return 1;
            case 'V': return 5;
            case 'X': return 10;
            case 'L': return 50;
            case 'C': return 100;
            case 'D': return 500;
            case 'M': return 1000;
            default: throw new FormatException($"Недопустимый символ: {c}");
        }
    }

    public static implicit operator RomanNumeral(int value)
    {
        return new RomanNumeral(value);
    }

    public static explicit operator int(RomanNumeral roman)
    {
        return roman.Value;
    }

    public static implicit operator RomanNumeral(string text)
    {
        return new RomanNumeral(FromRoman(text));
    }

    public static explicit operator string(RomanNumeral roman)
    {
        return roman.Value == 0 ? "" : ToRoman(roman.Value);
    }

    public override string ToString()
    {
        return (string)this;
    }
}

// 2
struct Percentage
{
    public double Fraction { get; }

    public Percentage(double fraction)
    {
        Fraction = fraction;
    }

    public static implicit operator Percentage(double value)
    {
        return new Percentage(value);
    }

    public static implicit operator Percentage(decimal value)
    {
        return new Percentage((double)value);
    }

    public static explicit operator double(Percentage p)
    {
        return p.Fraction;
    }

    public static explicit operator float(Percentage p)
    {
        return (float)p.Fraction;
    }

    public override string ToString()
    {
        return (Fraction * 100).ToString("0.##", CultureInfo.InvariantCulture) + "%";
    }
}

// 3
struct MemorySize
{
    private static readonly string[] Units = { "B", "KB", "MB", "GB", "TB" };

    public long Bytes { get; }

    public MemorySize(long bytes)
    {
        if (bytes < 0)
            throw new ArgumentOutOfRangeException(nameof(bytes), "Размер не может быть отрицательным.");
        Bytes = bytes;
    }

    private static long Parse(string text)
    {
        var match = Regex.Match(
            text,
            @"^\s*(\d+(?:[.,]\d+)?)\s*(B|KB|MB|GB|TB)\s*$",
            RegexOptions.IgnoreCase);

        if (!match.Success)
            throw new FormatException($"Некорректный размер памяти: {text}");

        decimal number = decimal.Parse(
            match.Groups[1].Value.Replace(',', '.'),
            CultureInfo.InvariantCulture);

        int power = Array.IndexOf(Units, match.Groups[2].Value.ToUpperInvariant());
        decimal multiplier = 1;
        for (int i = 0; i < power; i++)
            multiplier *= 1024;

        return (long)(number * multiplier);
    }

    public static implicit operator MemorySize(long bytes)
    {
        return new MemorySize(bytes);
    }

    public static explicit operator long(MemorySize size)
    {
        return size.Bytes;
    }

    public static implicit operator MemorySize(string text)
    {
        return new MemorySize(Parse(text));
    }

    public static explicit operator string(MemorySize size)
    {
        double value = size.Bytes;
        int unit = 0;
        while (value >= 1024 && unit < Units.Length - 1)
        {
            value /= 1024;
            unit++;
        }
        return value.ToString("0.##", CultureInfo.InvariantCulture) + " " + Units[unit];
    }

    public override string ToString()
    {
        return (string)this;
    }
}

// 4
struct PhoneNumber
{
    public long Digits { get; }

    public PhoneNumber(long digits)
    {
        int length = digits.ToString(CultureInfo.InvariantCulture).Length;
        if (digits <= 0 || length < 10 || length > 15)
            throw new ArgumentException("Номер должен содержать от 10 до 15 цифр.");
        Digits = digits;
    }

    private static long Parse(string text)
    {
        string cleaned = Regex.Replace(text, @"[\s\-()]", "");
        if (!Regex.IsMatch(cleaned, @"^\+?\d{10,15}$"))
            throw new FormatException($"Некорректный номер телефона: {text}");
        return long.Parse(cleaned.TrimStart('+'), CultureInfo.InvariantCulture);
    }

    public static implicit operator PhoneNumber(string text)
    {
        return new PhoneNumber(Parse(text));
    }

    public static explicit operator string(PhoneNumber phone)
    {
        return "+" + phone.Digits.ToString(CultureInfo.InvariantCulture);
    }

    public static explicit operator long(PhoneNumber phone)
    {
        return phone.Digits;
    }

    public static explicit operator PhoneNumber(long digits)
    {
        return new PhoneNumber(digits);
    }

    public override string ToString()
    {
        return (string)this;
    }
}

// 5
struct Angle
{
    public double Degrees { get; }

    public Angle(double degrees)
    {
        Degrees = degrees;
    }

    public static implicit operator Angle(double degrees)
    {
        return new Angle(degrees);
    }

    public static explicit operator double(Angle angle)
    {
        return angle.ToRadians();
    }

    public static implicit operator Angle((double value, bool isRadians) source)
    {
        return source.isRadians
            ? new Angle(source.value * 180.0 / Math.PI)
            : new Angle(source.value);
    }

    public double ToDegrees()
    {
        return Degrees;
    }

    public double ToRadians()
    {
        return Degrees * Math.PI / 180.0;
    }

    public override string ToString()
    {
        return Degrees.ToString("0.####", CultureInfo.InvariantCulture) + "*";
    }
}

class Program
{
    static void Main()
    {
        // 1
        Console.WriteLine("1");
        RomanNumeral r = 1999;
        int value = (int)r;
        RomanNumeral r2 = "XIV";
        string str = (string)r2;
        Console.WriteLine($"{value} -> {r}");
        Console.WriteLine($"{str} -> {(int)r2}");

        // 2
        Console.WriteLine("\n2");
        Percentage tax = 0.20;
        Percentage discount = 0.15m;
        double asDouble = (double)tax;
        float asFloat = (float)discount;
        Console.WriteLine($"{tax}, double = {asDouble}");
        Console.WriteLine($"{discount}, float = {asFloat}");

        // 3
        Console.WriteLine("\n3");
        MemorySize mem = "2MB";
        long bytes = (long)mem;
        string pretty = (string)mem;
        MemorySize kb = 1024L;
        Console.WriteLine($"{bytes} byte, {pretty}");
        Console.WriteLine($"1024 -> {(string)kb}");
        Console.WriteLine($"3GB -> {(string)(MemorySize)"3GB"}");

        // 4
        Console.WriteLine("\n4");
        PhoneNumber phone = "+79231234567";
        long digits = (long)phone;
        PhoneNumber fromLong = (PhoneNumber)79001112233L;
        Console.WriteLine($"{(string)phone}, digits = {digits}");
        Console.WriteLine($"from long: {(string)fromLong}");

        // 5
        Console.WriteLine("\n5");
        Angle a = 90.0;
        double rad = (double)a;
        Angle b = (Math.PI, true);
        Angle c = (45.0, false);
        Console.WriteLine($"{a} = {Math.Round(rad, 4)} rad");
        Console.WriteLine($"{b}, ToRadians = {Math.Round(b.ToRadians(), 4)}");
        Console.WriteLine($"{c}, ToDegrees = {c.ToDegrees()}");
    }
}