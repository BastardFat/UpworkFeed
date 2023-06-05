using System.Text;
using System.Text.RegularExpressions;
using UpworkFeed.Core.Models;

namespace UpworkFeed.Bot.Formatters;

public class MarkdownFormatter : IBotFormatter<JobOffer>
{
    public string Format(JobOffer job)
    {
        var result = FormatCore(job);
        if (result.Length > 4000)
        {
            job.Description = job.Description[..^(result.Length - 4000)] + " ...";
            result = FormatCore(job);
        }
        return result;
    }

    public string FormatCore(JobOffer job)
    {
        StringBuilder sb = new();
        sb.AppendLine($"*{Escape(job.Title)}*");
        sb.AppendLine($"{Escape(job.Description)}");
        sb.AppendLine();
        if (job.Skills?.Length > 0)
        {
            sb.AppendLine($"Skills: _{string.Join(", ", job.Skills.Select(Escape))}_");
        }
        sb.AppendLine();
        foreach (var item in job.Attributes)
        {
            sb.AppendLine($"*{Escape(item.Key)}*: {Escape(item.Value)}");
        }
        sb.AppendLine($"[Link to this job]({Escape(job.Link)})");

        return sb.ToString();
    }

    public string Escape(string s)
    {
        char[] charsToEscape = new[] { '\\', '_', '*', '[', ']', '(', ')', '~', '`', '>', '+', '-', '=', '|', '{', '}', '.', '!' };
        Dictionary<string, string> namedCodes = new()
        {
            { "&nbsp;", "&#160;" },
            { "&quot;", "&#34;" },
            { "&amp;", "&#38;" },
            { "&apos;", "&#39;" },
            { "&lt;", "&#60;" },
            { "&gt;", "&#62;" },
            { "&cent;", "&#162;" },
            { "&pound;", "&#163;" },
            { "&yen;", "&#165;" },
            { "&euro;", "&#8364;" },
            { "&copy;", "&#169;" },
            { "&reg;", "&#174;" },
        };


        foreach (var c in charsToEscape)
            s = s.Replace($"{c}", $"\\{c}");

        foreach (var c in namedCodes)
            s = s.Replace(c.Key, c.Value);

        s = Regex.Replace(s, @"\&\#(\d+)\;", (m) => $"\\{(char)int.Parse(m.Groups[1].Value)}");
        s = s.Replace($"#", $"\\#");
        return s;
    }
}
