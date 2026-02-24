using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUITestProject.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sb = new StringBuilder();

            for (int i = 0; i < input.Length; i++) {
                char c = input[i];

                if (char.IsUpper(c))
                {
                    if (i > 0) sb.Append("_");
                    sb.Append(char.ToLower(c));
                }
                else
                {
                    sb.Append(c);
                }
            
            }
            return sb.ToString();
        }

        public static string ToPluralize(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            if (input.EndsWith("y")) return input[..^1] + "ies";
            return input + "s";

        }

    }
}
