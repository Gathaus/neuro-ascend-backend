namespace Neuro.Domain.Entities;

public class FoodPage : BaseEntity<int>
{
    public string Name { get; set; }   
    public string ImagePath { get; set; }
    public string VideoPath { get; set; }
    public int Calories { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public List<string> Materials { get; set; }
    public List<string> Instructions { get; set; }
    
}