namespace DotNetAPI.Core.Common.Extensions;

public static class StringExtensions
{
    public static string Format(this string text)
    {
        return string.Concat(text.Select(@char => char.IsUpper(@char) ? "_" + @char : @char.ToString())).ToUpper();
    }
}
