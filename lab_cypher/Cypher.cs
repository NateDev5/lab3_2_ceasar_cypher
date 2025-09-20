using System.Text;

namespace lab_cypher;

public static class Cypher
{
    public static string decrypt_text(string data, int shift)
    {
        StringBuilder builder = new StringBuilder();
        foreach (char c in data)
        {
            if (!char.IsAsciiLetter(c))
            {
                builder.Append(c);
                continue;
            }

            char start = char.IsUpper(c) ? 'A' : 'a';
            builder.Append((char)(((c - start - shift) % 26) + start));
        }

        return builder.ToString();
    }
}