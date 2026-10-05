using System;
using System.ComponentModel;

delegate string Greeter(string name);

// 5
class LimitEventArgs : EventArgs
{
    public int Value { get; }

    public LimitEventArgs(int value) => Value = value;
}

class Counter
{
    private int value = 0;

    public event EventHandler<LimitEventArgs> LimitReached;

    public void Increment()
    {
        value++;
        Console.WriteLine($"incr {value}");

        if (value == 5)
            LimitReached?.Invoke(this, new LimitEventArgs(value));
    }
}

// 6
class Process
{
    public event EventHandler<CancelEventArgs> Starting;

    public void Start()
    {
        var args = new CancelEventArgs();
        Starting?.Invoke(this, args);

        if (args.Cancel)
        {
            Console.WriteLine("передумал");
            return;
        }

        Console.WriteLine("запуск");
    }
}

class Program
{
    // 1
    static string Hello(string name) => $"Hello, {name}";
    static string Goodbye(string name) => $"Goodbye, {name}";

    // 2
    static void PrintUpper(string s) =>
        Console.WriteLine(s.ToUpper());

    static void PrintLength(string s) =>
        Console.WriteLine($"Длина: {s.Length}");

    static void PrintWithDate(string s) =>
        Console.WriteLine($"{DateTime.Now}: {s}");

    // 3
    static void Meth1() => Console.WriteLine("1");

    static void Meth2()
    {
        Console.WriteLine("2");
        throw new Exception("Durakey");
    }

    static void Meth3() => Console.WriteLine("3");

    static void Meth4() => Console.WriteLine("4");

    // 4
    static string GetString() => "стринг";

    static void PrintObject(object o) =>
        Console.WriteLine($"чупеп {o}");

    // 5
    static void OnLimitReached(object sender, LimitEventArgs e) =>
        Console.WriteLine($"уже многа - {e.Value}");

    // 6
    static void OnStartingCancel(object sender, CancelEventArgs e)
    {
        Console.WriteLine("фанат отменяет запуск");
        e.Cancel = true;
    }

    static void Main()
    {
        // 1
        Greeter act = Hello;
        Console.WriteLine(act("Ishak"));

        act = Goodbye;
        Console.WriteLine(act("Ishak"));

        // 2
        Action<string> act2 = PrintUpper;
        act2 += PrintLength;
        act2 += PrintWithDate;

        act2("sueta");

        // 3
        Action blockchainGosha = Meth1;
        blockchainGosha += Meth2;
        blockchainGosha += Meth3;
        blockchainGosha += Meth4;

        foreach (Action d in blockchainGosha.GetInvocationList())
        {
            try
            {
                d();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Пойман на ошибке, мортис бравл старс байт на тыки ");
            }
        }

        // 4
        Func<object> fuc = GetString;
        Console.WriteLine(fuc());

        Action<string> act3 = PrintObject;
        act3("salam");

        // 5
        Counter counter = new Counter();
        counter.LimitReached += OnLimitReached;

        for (int i = 0; i < 6; i++)
            counter.Increment();

        // 6
        Process p1 = new Process();
        p1.Starting += OnStartingCancel;
        p1.Start();

        Process p2 = new Process();
        p2.Start();
    }
}