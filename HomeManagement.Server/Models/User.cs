using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HomeManagement.Server.Models;

[Table( "Users" )]
public class User : BasicTable
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }
    
    [Required]
    public string Password { get; set; }

    [Required]
    [JsonIgnore]
    public Family Family { get; set; }

    public bool Active { get; set; } = false;

    [JsonIgnore]
    public string FullName =>
        $"{FirstName} {LastName}";

    [JsonIgnore]
    public UserDetails SafeResponse =>
        new()
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            PhoneNumber = PhoneNumber,
        };
}

public class UserDetails
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
}