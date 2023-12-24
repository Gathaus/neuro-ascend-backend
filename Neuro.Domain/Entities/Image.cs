using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neuro.Domain.Entities;

public class Image
{
    [Key]
    public int ImageId { get; set; }

    public string? ImagePath { get; set; }

}