using System.Text;

namespace lab_cypher;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Too much/Not enough arguments provided");
            return;
        }
        
        string training_file_path = args[0];
        string scambled_file_path = args[1];
        string output_file_path = args[2];

        if (!File.Exists(training_file_path))
        {
            Console.WriteLine("Invalid training file path");
            return;
        }

        if (!File.Exists(scambled_file_path))
        {
            Console.WriteLine("Invalid scrambled file path");
            return;
        }

        Console.WriteLine($"Reading input file \"{training_file_path}\"");
        Dictionary<char, int> training_occuring_letters = get_letters_occurences_from_file(training_file_path);
        
        KeyValuePair<char, int> training_most_occuring = training_occuring_letters.MaxBy(e => e.Value);
        Console.WriteLine($"The most occuring character is {training_most_occuring.Key}, occuring {training_most_occuring.Value} times");
        
        string scrambled_file_data;
        using (StreamReader reader = new StreamReader(scambled_file_path))
            scrambled_file_data = reader.ReadToEnd();
        
        Console.WriteLine($"Reading the encrypted file \"{training_file_path}\"");
        Dictionary<char, int> scrambled_occuring_letters = get_letters_occurences_from_data(scrambled_file_data);
        
        KeyValuePair<char, int> scrambled_most_occuring = scrambled_occuring_letters.MaxBy(e => e.Value);
        Console.WriteLine($"The most occuring character is {scrambled_most_occuring.Key}, occuring {scrambled_most_occuring.Value} times");

        int shift = training_most_occuring.Key - scrambled_most_occuring.Key;
        
        Console.WriteLine($"A shift factor of {shift} has been determined");
        
        string decrypted = Cypher.decrypt_text(scrambled_file_data, shift);

        Console.WriteLine($"Writing output file now to \"{output_file_path}\"");
        using (StreamWriter writer = new StreamWriter(output_file_path))
            writer.Write(decrypted);
        
        Console.WriteLine("Display the file? (y/n)");
        string? choice = Console.ReadLine();

        if (choice == null || choice.Length > 1 || choice.ToLower() != "y")
        {
            Console.WriteLine("Ending program");
            return;
        }
        
        Console.WriteLine(decrypted);
    }

    static Dictionary<char, int> get_letters_occurences_from_file(string file_path)
    {
        Dictionary<char, int> occuring_letters = new Dictionary<char, int>();
        using (StreamReader reader = new StreamReader(file_path))
        {
            while (!reader.EndOfStream)
            {
                char cur_char = (char)reader.Read();

                if(cur_char == ' ') continue;
                
                if (!occuring_letters.ContainsKey(cur_char))
                    occuring_letters.Add(cur_char, 0);
                
                occuring_letters[cur_char]++;
            }
        }

        return occuring_letters;
    }
    
    static Dictionary<char, int> get_letters_occurences_from_data(string data)
    {
        Dictionary<char, int> occuring_letters = new Dictionary<char, int>();
        foreach (char cur_char in data)
        {
            if(cur_char == ' ') continue;
                
            if (!occuring_letters.ContainsKey(cur_char))
                occuring_letters.Add(cur_char, 0);
                
            occuring_letters[cur_char]++;
        }

        return occuring_letters;
    }
}