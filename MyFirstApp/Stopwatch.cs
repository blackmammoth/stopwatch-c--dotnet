public class Stopwatch
{
    // Fields
    private int timeElapsed;
    private bool isRunning;

    // Properties
    public int TimeElapsed => timeElapsed;
    public bool IsRunning => isRunning;

    // Delegates and Events
    public delegate void StopwatchEventHandler(string message);
    public event StopwatchEventHandler OnStarted;
    public event StopwatchEventHandler OnStopped;
    public event StopwatchEventHandler OnReset;

    // Methods
    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            OnStarted?.Invoke("Stopwatch Started!");
        }
    }

    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            OnStopped?.Invoke("Stopwatch Stopped!");
        }
    }

    public void Reset()
    {
        bool wasRunning = isRunning;
        isRunning = false;
        timeElapsed = 0;
        OnReset?.Invoke("Stopwatch Reset!");

        if (wasRunning)
        {
            OnStopped?.Invoke("Stopwatch Stopped (as part of reset)!");
        }
    }

    public void Tick()
    {
        if (isRunning)
        {
            timeElapsed++;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

        // Subscribe to events
        stopwatch.OnStarted += message => Console.WriteLine(message);
        stopwatch.OnStopped += message => Console.WriteLine(message);
        stopwatch.OnReset += message => Console.WriteLine(message);

        // Console UI
        bool exit = false;

        Console.WriteLine("Stopwatch Console UI");
        Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit.");

        Thread timerThread = new Thread(() =>
        {
            while (!exit)
            {
                if (stopwatch.IsRunning)
                {
                    stopwatch.Tick();
                    Console.WriteLine($"Time Elapsed: {stopwatch.TimeElapsed} seconds");
                }
                Thread.Sleep(1000);
            }
        });

        timerThread.Start();

        while (!exit)
        {
            ConsoleKey key = Console.ReadKey(intercept: true).Key;

            switch (key)
            {
                case ConsoleKey.S:
                    stopwatch.Start();
                    break;
                case ConsoleKey.T:
                    stopwatch.Stop();
                    break;
                case ConsoleKey.R:
                    stopwatch.Reset();
                    break;
                case ConsoleKey.Q:
                    exit = true;
                    stopwatch.Stop();
                    break;
                default:
                    Console.WriteLine("Invalid input. Use S, T, R, or Q.");
                    break;
            }
        }

        timerThread.Join();
        Console.WriteLine("Exiting Stopwatch Console UI. Goodbye!");
    }
}
