namespace Calculator.ConsoleApp;

public interface ICustomConsole
{
    void WriteLine(string message = "");
    string Prompt(string message);
    char PromptKey(string message);
}