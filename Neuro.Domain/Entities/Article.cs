namespace Neuro.Domain.Entities;

public class Article : BaseEntity<int>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string ArticleImagePath { get; set; }
    public string AuthorImagePath { get; set; }
}