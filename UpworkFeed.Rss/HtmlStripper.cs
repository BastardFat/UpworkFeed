using System.Text.RegularExpressions;

namespace UpworkFeed.Rss;

internal class HtmlStripper
{
    public Dictionary<string, string> ExtractAttributes(string description) =>
        Regex.Matches(description, @"<b>(.+?)</b>: *(.+?)<br />")
            .ToDictionary(m => m.Groups[1].Value.Trim(), m => m.Groups[2].Value.Trim());

    public string RemoveExtraWhitespaces(string description) =>
        Regex.Replace(description, @"\s+", " ");

    public string StripDescription(string description)
    {
        description = Regex.Replace(description, @"<b>.+</b>: *.+?<br />", "");
        description = Regex.Replace(description, @"<br\s*/>", "\n");
        description = Regex.Replace(description, @"<a href=""(.+?)"">click to apply</a>", "");
        return description;
    }
}
