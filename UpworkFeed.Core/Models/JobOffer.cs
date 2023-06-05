namespace UpworkFeed.Core.Models;

public class JobOffer
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Skills { get; set; }
    public Dictionary<string, string> Attributes { get; set; }
    public string Link { get; set; }
    public string SourceDescriptionText { get; set; }
}
