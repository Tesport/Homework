using System;
using System.Text.RegularExpressions;

// 1
class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public (string Title, string Author, int Year) ToTuple()
    {
        return (Title, Author, Year);
    }
}

// 2
class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point((int x, int y) tuple)
    {
        X = tuple.x;
        Y = tuple.y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

class Program
{
    // 3
    static (int, string, double) GetData(int number)
    {
        return (number, $"Число {number}", number / 2.0);
    }

    // 7
    static (string Name, (string City, string Street) Address, (int Year, string Month, int Day) Birth) ParsePerson(string input)
    {
        var match = Regex.Match(
            input,
            @"^\s*(.+?)\s*,\s*\(\s*(.+?)\s*,\s*(.+?)\s*\)\s*,\s*\(\s*(\d+)\s*,\s*(.+?)\s*,\s*(\d+)\s*\)\s*$");

        if (!match.Success)
            throw new FormatException("Неверный формат строки.");

        return (
            match.Groups[1].Value,
            (match.Groups[2].Value, match.Groups[3].Value),
            (int.Parse(match.Groups[4].Value), match.Groups[5].Value, int.Parse(match.Groups[6].Value))
        );
    }

    static void Main()
    {
        Console.WriteLine("Задание 1");
        var book = new Book("Мастер и Маргарита", "Булгаков", 1967);
        var bookTuple = book.ToTuple();
        Console.WriteLine($"{bookTuple.Title}, {bookTuple.Author}, {bookTuple.Year}");

        Console.WriteLine("\nЗадание 2");
        var point = new Point((3, 7));
        Console.WriteLine(point);

        Console.WriteLine("\nЗадание 3");
        var (_, text, _) = GetData(5);
        Console.WriteLine(text);

        Console.WriteLine("\nЗадание 4");
        (string Name, int Grade)[] students =
        {
            ("Мария", 5),
            ("Борис", 4),
            ("Анна", 3),
            ("Виктор", 5),
            ("Дарья", 4)
        };
        Array.Sort(students, (x, y) => string.Compare(x.Name, y.Name, StringComparison.CurrentCulture));
        foreach (var student in students)
            Console.WriteLine($"{student.Name}: {student.Grade}");

        Console.WriteLine("\nЗадание 5");
        var first = (1, "один");
        var second = (2.5, 'x', true);
        var combined = (first, second);
        Console.WriteLine($"{combined.Item1.Item1}, {combined.Item1.Item2}");
        Console.WriteLine($"{combined.Item2.Item1}, {combined.Item2.Item2}, {combined.Item2.Item3}");

        Console.WriteLine("\nЗадание 6");
        var t1 = (1, "abc", 2.5);
        var t2 = (1, "abc", 2.5);
        var t3 = (1, "abd", 2.5);
        var named = (Id: 1, Text: "abc", Value: 2.5);
        Console.WriteLine($"t1 == t2: {t1 == t2}");
        Console.WriteLine($"t1 == t3: {t1 == t3}");
        Console.WriteLine($"t1 != t3: {t1 != t3}");
        Console.WriteLine($"t1.Equals(t2): {t1.Equals(t2)}");
        Console.WriteLine($"t1 == named: {t1 == named}");
        Console.WriteLine("Вывод: кортежи равны, если равны все соответствующие элементы по порядку.");
        Console.WriteLine("Вывод: достаточно отличия в одном элементе, чтобы кортежи стали неравными.");
        Console.WriteLine("Вывод: имена элементов не влияют на сравнение, важны только значения и порядок.");

        Console.WriteLine("\nЗадание 7");
        string input = "Иван, (Москва, Ленина), (1990, Май, 15)";
        var person = ParsePerson(input);
        Console.WriteLine(
            $"{person.Name} проживает в {person.Address.City} {person.Address.Street}, " +
            $"родился в {person.Birth.Day}, {person.Birth.Month}, {person.Birth.Year}");
    }
}