using HomeManagement.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Server.Controllers.VersionOne;

[ApiController, Route( "api/v1/users" )]
public class UserController : BaseVersionOneController
{
    public UserController( DatabaseContext ctx ) : base( ctx ) { }

    [HttpGet]
    public IActionResult Get()
    {
        var list = new List<UserDetails>();
        foreach ( var details in Database.Users.Select( user => user.SafeResponse ) ) list.Add( details );
        return Ok( list );
    }

    [HttpGet( "{id}" )]
    public IActionResult Get( string id )
    {
        var user = Database.Users.FirstOrDefault( x => x.Id == Guid.Parse( id ) );

        if ( user is not null )
            return Ok( user.SafeResponse );

        return NotFound();
    }

    [HttpDelete( "{id}" )]
    [Authorize]
    public async Task<IActionResult> Delete( string id )
    {
        if ( !ModelState.IsValid )
            return NotFound( ModelState );
        
        // Only allow the user to delete their own account
        if ( CurrentUser?.Id != Guid.Parse( id ) )
            return Unauthorized();

        var user = Database.Users.FirstOrDefault( x => x.Id == Guid.Parse( id ) );

        if ( user is null )
            return NotFound( id );

        Database.Users.Remove( user );
        await Database.SaveChangesAsync();

        return Ok();

    }

    // [HttpPut]
    // public async Task<IActionResult> Update( [FromBody] UserDetails details )
    // {
    //     // TODO This
    //     return Ok();
    // }
}