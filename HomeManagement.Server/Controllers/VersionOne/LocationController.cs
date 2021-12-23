using HomeManagement.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Server.Controllers.VersionOne;

[ApiController, Route( "api/v1/locations" )]
public class LocationController : BaseVersionOneController
{
    public LocationController( DatabaseContext ctx ) : base( ctx ) { }

    [HttpGet]
    public IActionResult GetAll()
    {
        // TODO Get all in the logged in user's family
        return Ok( Database.Locations );
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create( [FromBody] LocationDetails details )
    {
        if ( !ModelState.IsValid )
            return BadRequest( ModelState );

        if ( CurrentUser?.Family is null )
            return Unauthorized();

        // TODO This can be improved
        if ( details.Name is null || details.Longitude is null || details.Latitude is null || details.Radius is null )
            return BadRequest();

        var location = new Location
        {
            Name = details.Name,
            Latitude = (float) details.Latitude,
            Longitude = (float) details.Longitude,
            Radius = (float) details.Radius
        };

        location.Created();
        location.Family = CurrentUser.Family;
        
        CurrentUser.Family.Locations.Add( location );
        await Database.SaveChangesAsync();

        return Ok( location );
    }

    [HttpPut]
    public async Task<IActionResult> Update( [FromBody] LocationDetails details )
    {
        if ( !ModelState.IsValid )
            return BadRequest( ModelState );

        if ( CurrentUser?.Family is null )
            return Unauthorized();
        
        if ( details.Id is null )
            return BadRequest();

        var location = Database.Locations.FirstOrDefault( x => x.Id == Guid.Parse( details.Id ) );

        if ( location is null )
            return NotFound();

        location.Name = details.Name ?? location.Name;
        location.Latitude = details.Latitude ?? location.Latitude;
        location.Longitude = details.Longitude ?? location.Longitude;
        location.Radius = details.Radius ?? location.Radius;

        location.Modified();
        
        await Database.SaveChangesAsync();
        return Ok( location );
    }

    [HttpDelete( "{id}" )]
    public async Task<IActionResult> Delete( string id )
    {
        if ( !ModelState.IsValid )
            return BadRequest( ModelState );

        if ( CurrentUser?.Family is null )
            return Unauthorized();

        var location = Database.Locations.FirstOrDefault( x => x.Id == Guid.Parse( id ) );

        if ( location is null )
            return NotFound();
        
        Database.Locations.Remove( location );
        await Database.SaveChangesAsync();
        
        return Ok();
    }
}

public class LocationDetails
{
    public string? Id { get; set; }
    
    public string? Name { get; set; }
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
    public float? Radius { get; set; }
}