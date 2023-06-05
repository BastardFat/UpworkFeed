using CodeHollow.FeedReader;
using UpworkFeed.Core;
using UpworkFeed.Core.Models;

namespace UpworkFeed.Rss;

public class JobOfferConverter : IConverter<string, FeedItem, JobOffer>
{
    public string GetKey(FeedItem source) => source.Id;
    public JobOffer Convert(FeedItem item)
    {
        var stripper = new HtmlStripper();
        var job = new JobOffer
        {
            SourceDescriptionText = item.Description,
            Id = item.Id,
            Title = item.Title,
            Link = item.Link.Replace("?source=rss", "")
        };

        var description = stripper.RemoveExtraWhitespaces(item.Description);

        job.Attributes = stripper.ExtractAttributes(description);
        job.Description = stripper.StripDescription(description);

        if (job.Attributes.TryGetValue("Skills", out var skills))
        {
            job.Skills = skills.Split(",").Select(x => x.Trim()).ToArray();
            job.Attributes.Remove("Skills");
        }

        return job;
    }
}
