namespace Neuro.Api.Models;

public class UserMedicineRequest
{
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public bool IsTaken { get; set; }

}