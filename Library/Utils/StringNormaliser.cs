using System;

namespace Library.Utils
{
    public class StringNormaliser
    {
        static private string ReplaceAsSpan(ReadOnlySpan<char> input)
        {
            Span<char> resSpan = stackalloc char[input.Length];

            for (var i = 0; i < input.Length; i++)
            {
                resSpan[i] = input[i] switch
                {
                    'а' => 'a',
                    'в' => 'b',
                    'с' => 'c',
                    'е' => 'e',
                    'н' => 'h',
                    'к' => 'k',
                    'м' => 'm',
                    'о' => 'o',
                    'р' => 'p',
                    'т' => 't',
                    'х' => 'x',
                    'у' => 'y',
                    _ => input[i]
                };
            }

            return resSpan.ToString();
        }

       
        public static string NormalizeString(string input)
        {
            int size = 0;
            Span<char> resSpan = stackalloc char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) || Char.IsNumber(input[i]))
                {
                    resSpan[size] = Char.ToLower(input[i]);
                    size++;
                }
            }

            return ReplaceAsSpan(resSpan.Slice(0, size));
        }

    }
}
