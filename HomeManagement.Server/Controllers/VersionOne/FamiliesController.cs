using HomeManagement.Server.Extensions;
using HomeManagement.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Server.Controllers.VersionOne;

[ApiController, Route( "api/v1/families" )]
public class FamiliesController : BaseVersionOneController
{
    public FamiliesController( DatabaseContext ctx ) : base( ctx ) { }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok( Database.Families );
    }

    [HttpPost( "{password}/{invite}" )]
    public async Task<IActionResult> Create( string password, string invite )
    {
        if ( !ModelState.IsValid )
            return BadRequest( ModelState );
        
        // TODO Find a way to auth this

        var family = new Family
        {
            InviteCode = invite,
            MasterPassword = password.Hash()
        };

        family.Created();

        Database.Families.Add( family );
        await Database.SaveChangesAsync();

        return Ok( family );
    }

    [HttpPut( "{password}/{invite}" )]
    public async Task<IActionResult> Update( string password, string invite )
    {
        if ( !ModelState.IsValid )
            return NotFound( ModelState );

        var family = CurrentUser?.Family;

        if ( family is null )
            return Unauthorized();

        if ( family.MasterPassword != password.Hash() )
            return Unauthorized();

        family.InviteCode = invite;
        family.Modified();
        
        await Database.SaveChangesAsync();

        return Ok();
    }
}