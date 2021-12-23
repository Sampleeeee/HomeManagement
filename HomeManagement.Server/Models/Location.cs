using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HomeManagement.Server.Models;

[Table( "Locations" )]
public class Location : OwnedTable
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    [JsonIgnore]
    public Family Family { get; set; }

    [Required]
    public float Latitude { get; set; }
    
    [Required]
    public float Longitude { get; set; }
    
    [Required]
    public float Radius { get; set; }

    [JsonIgnore]
    public Coordinate Coordinates => 
        new Coordinate( Latitude, Longitude );
}

public class Coordinate
{
    public float Longitude { get; set; }

    public float Latitude { get; set; }

    public Coordinate( float longitude, float latitude )
    {
        Longitude = longitude;
        Latitude = latitude;
    }
}