using System.Text;
using UpworkFeed.Core.Models;

namespace UpworkFeed.Bot.Formatters;

public class HtmlFormatter : IBotFormatter<JobOffer>
{
    public string Format(JobOffer job)
    {
        var desc = job.SourceDescriptionText;
        if (desc.Length > (4000 - job.Title.Length))
            desc = desc[..(4000 - job.Title.Length)] + " ...";

        StringBuilder sb = new();
        sb.AppendLine(job.Title);
        sb.AppendLine(desc);

        return sb.ToString();
    }
}
