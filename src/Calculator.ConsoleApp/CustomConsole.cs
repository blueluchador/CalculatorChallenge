namespace Calculator.ConsoleApp;

public class CustomConsole : ICustomConsole
{
    public void WriteLine(string message = "") => Console.WriteLine(message);

    public string Prompt(string message)
    {
        Console.Write(message);
        return Console.ReadLine()?.Trim() ?? String.Empty;
    }

    public char PromptKey(string message)
    {
        Console.Write(message);
        return Console.ReadKey(intercept: true).KeyChar;
    }
}