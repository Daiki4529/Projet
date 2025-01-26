using CsvHelper.Configuration.Attributes;

public class NetflixTitle
{
    [Name("show_id")]
    public string? ShowId { get; set; }
    
    [Name("type")]
    public string? Type { get; set; }
    
    [Name("title")]
    public string? Title { get; set; }
    
    [Name("director")]
    public string? Director { get; set; }
    
    [Name("cast")]
    public string? Cast { get; set; }
    
    [Name("country")]
    public string? Country { get; set; }
    
    [Name("date_added")]
    public DateTime? DateAdded { get; set; }
    
    [Name("release_year")]
    public int? ReleaseYear { get; set; }
    
    [Name("rating")]
    public string? Rating { get; set; }
    
    [Name("duration")]
    public string? Duration { get; set; }
    
    [Name("listed_in")]
    public string? ListedIn { get; set; }
    
    [Name("description")]
    public string? Description { get; set; }
}
