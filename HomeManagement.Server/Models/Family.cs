using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HomeManagement.Server.Models;

[Table( "Families" )]
public class Family : BasicTable
{
    [Required]
    public string InviteCode { get; set; }
    
    /// <summary>
    /// The main password to the family, allows access to logs and development tools
    /// </summary>
    [Required]
    public string MasterPassword { get; set; }

    [JsonIgnore]
    public ICollection<User> Users { get; set; }
    
    [JsonIgnore]
    public ICollection<Location> Locations { get; set; }
}