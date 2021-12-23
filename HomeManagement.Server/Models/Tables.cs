using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Server.Models;

public abstract class BasicTable
{
    [Key] 
    public Guid Id { get; set; } = Guid.Empty;
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public DateTime ModifiedAt { get; set; }

    public virtual void Created( bool keepId = false )
    {
        if ( !keepId )
            Id = Guid.NewGuid();
        
        CreatedAt = DateTime.Now;
        ModifiedAt = DateTime.Now;
    }

    public virtual void Modified()
    {
        ModifiedAt = DateTime.Now;
    }
}

public abstract class OwnedTable : BasicTable
{
    [Required]
    public Family Family { get; set; }
    
    // [Required]
    // public string FamilyId { get; set; }
}