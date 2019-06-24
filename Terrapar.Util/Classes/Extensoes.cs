using System.Text.RegularExpressions;

namespace Terrapar.Classes
{
    public static class Extensoes
    {
        public static string PrimeiroNome(this string nome)
        {
            try
            {
                return nome.Split(' ')[0];
            }
            catch
            {
                return nome;
            }
        }

        public static string ToStringSegura(this string str)
        {
            return str == null ? null : Regex.Replace(str, @"[^a-zA-Z0-9_.\s\w-]+", string.Empty); // \w => qqr caracter (menos especial), \s => espaço
        }

        public static string ToDecimalMySql(this decimal str)
        {
            return str.ToString().Replace(",", ".");

        }

        public static string ToIntMySql(this int str)
        {
            return str.ToString().Replace(".", "");

        }
    }
}