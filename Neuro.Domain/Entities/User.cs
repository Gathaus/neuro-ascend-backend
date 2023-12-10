namespace Neuro.Domain.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}