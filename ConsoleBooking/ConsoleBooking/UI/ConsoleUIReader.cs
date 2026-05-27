namespace ConsoleBooking.UI;

public class ConsoleUIReader
{
    public delegate bool TryParseDelegate<T>(string input, out T value);

    public T ReadValue<T>(string message, TryParseDelegate<T> parser)
    {
        Console.WriteLine(message);
        while (true)
        {
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input is empty");
                continue;
            }

            if (parser(input, out var value))
                return value;

            Console.WriteLine("Incorrect input");
        }
    }

    public string ReadString(string message)
    {
        while (true)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.WriteLine("Incorrect input");
        }
    }

    public bool ReadBool(string message)
    {
        while (true)
        {
            Console.WriteLine(message + " 1 - True, 2 - False");
            var input = Console.ReadLine();
            if (input == "1")
                return true;
            if (input == "2")
                return false;
            Console.WriteLine("Incorrect input");
        }
    }
}