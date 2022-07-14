using System.Text;

namespace BullsAndCowsConsoleApp;

internal class Program
{
    private const string KEYWORD_BULL = "BULL";
    private const string KEYWORD_COW = "COW";
    
    static void Main(string[] args)
    {
        // Alphabet: 0 1 2 3 4 5 6 7 8 9
        char[] secretKeyAlphabet = Enumerable.Range(0, 10).Select(x => x.ToString()[0]).ToArray();
        int secretKeyLength = 4;
        int attempts = 10;
        
        string secretKey = GenerateSecretKey(secretKeyAlphabet, secretKeyLength);
        
        Console.WriteLine($"BULLS AND COWS");
        Console.WriteLine($"Enter a {secretKeyLength}-digit key:");
        while (ReadInput(secretKeyAlphabet, secretKeyLength) is { } input)
        {
            if (input == secretKey)
            {
                Console.WriteLine("You're right!");
                Console.ReadLine();
                break;
            }

            for (int i = 0; i < input.Length; i++)
            {
                char inputChar = input[i];
                if (IsBull(secretKey, inputChar, i))
                    Console.WriteLine($"{KEYWORD_BULL} {inputChar}");
                else if (IsCow(secretKey, inputChar))
                    Console.WriteLine($"{KEYWORD_COW} {inputChar}");
            }
            

            attempts--;
            if (attempts > 0)
            {
                Console.WriteLine($"{attempts} attempts left");
            }
            else
            {
                Console.WriteLine("You have not attempts. Try again later!");
                Console.ReadLine();
                break;
            }
        }
    }

    static string GenerateSecretKey(
        char[] alphabet,
        int length,
        Random? random = null)
    {
        if (length > alphabet.Length) 
            throw new InvalidOperationException(
                $"Secret key length should be less or equal to the length of the alphabet");
        random ??= new Random();
        IEnumerable<char> shuffledAlphabet = alphabet.OrderBy(x => random.Next());
        return string.Concat(shuffledAlphabet.Take(length));
    }

    static string? ReadInput(char[] alphabet, int length)
    {
        while (Console.ReadLine() is { Length: > 0 } input)
        {
            char[] invalidChars = input.Where(x => !alphabet.Contains(x)).Distinct().ToArray();
            if (invalidChars.Any())
            {
                Console.WriteLine($"Invalid symbols: {string.Join(", ", invalidChars)}. Please, try again");
                continue;
            }
            
            if (input.Length != length)
            {
                Console.WriteLine($"Invalid input length, {length} symbols required. Please, try again");
                continue;
            }

            return input;
        }

        return null;
    }

    static bool IsBull(string secretKey, char input, int position)
    {
        if (position >= secretKey.Length)
            throw new IndexOutOfRangeException(
                $"Cannot check {KEYWORD_BULL}: position should be less or equal to the length of the alphabet");
        
        return secretKey[position] == input;
    }

    static bool IsCow(string secretKey, char input)
    {
        return secretKey.Contains(input);
    }
}