namespace WebCrawler.Console.Services;

public class ConsoleService
{
    public virtual string ReadLine()
    {
        return System.Console.ReadLine();
    }

    public virtual void WriteLine(string message)
    {
        System.Console.WriteLine(message);
    }
}
