using System;
using System.Collections.Generic;
using System.Linq;

public class Student
{
    public string Name { get; set; }
    public string Faculty { get; set; }
    public List<int> Grades { get; set; }
}

public class Program
{
    private static List<Student> _testStudents = new List<Student>
    {
        new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
        new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
        new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } }
    };

    public static void Main()
    {
        // 1
        string target = "ФИТ";
        var result = _testStudents.Where(s => s.Faculty == target).ToList();

        foreach (var s in result)
        {
            Console.WriteLine(s.Name);
        }
        Console.WriteLine();

        // 2
        var pivot = 4;
        var result2 = _testStudents.Where(s => s.Grades.Average() >= pivot).ToList();

        foreach (var s in result2)
        {
            Console.WriteLine(s.Name);
        }
        Console.WriteLine();

        // 3
        var result3 = _testStudents.OrderBy(s => s.Name).ToList();

        foreach (var s in result3)
        {
            Console.WriteLine(s.Name);
        }
        Console.WriteLine();

        // 4
        var result4 = _testStudents.GroupBy(s => s.Faculty).ToList();

        foreach (var group in result4)
        {
            Console.WriteLine(group.Key);
            foreach (var s in group)
            {
                Console.WriteLine($"\t{s.Name}");
            }
        }
        Console.WriteLine();

        //5
        var result5 = _testStudents
            .GroupBy(s => s.Faculty)
            .Select(g => new
            {
                Faculty = g.Key,
                Avg = g.SelectMany(s => s.Grades).Average()
            })
            .ToList();

        foreach (var group in result5)
        {
            Console.WriteLine($"{group.Faculty}: {group.Avg:F2}");
        }
    }
}