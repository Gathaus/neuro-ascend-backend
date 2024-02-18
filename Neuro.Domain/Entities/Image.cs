using System.ComponentModel.DataAnnotations;

namespace Neuro.Domain.Entities;

public class Image
{
    [Key]
    public int ImageId { get; set; }

    public string? ImagePath { get; set; }

}