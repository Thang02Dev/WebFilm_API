using System.Text.RegularExpressions;
using System.Text;

namespace WebFilm_API.Commons
{
    public class ConvertDatas
    {
        public static string ConvertToSlug(string text)
        {
            text = text.ToLower();
#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
            text = Regex.Replace(text, @"\s", "-");
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
            text = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(text));
            return text;
        }
    }
}
