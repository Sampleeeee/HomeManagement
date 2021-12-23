using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HomeManagement.Server.Extensions;
using HomeManagement.Server.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeManagement.Server.Controllers.VersionOne;

[ApiController, Route( "api/v1/auth" )]
public class AuthenticationController : BaseVersionOneController
{
    public AuthenticationController( DatabaseContext ctx ) : base( ctx ) { }

    [HttpPost, Route( "login" )]
    public IActionResult Login( [FromBody] LoginDetails login )
    {
        if ( !ModelState.IsValid )
            return BadRequest( ModelState );

        if ( login.Email is null && login.PhoneNumber is null )
            return BadRequest();
        
        var user = Database.Users.FirstOrDefault( x => x.Email == login.Email || x.PhoneNumber == login.PhoneNumber );
        
        // TODO Dont leave these saying what is wrong
        
        if ( user is null )
            return NotFound( "User not found" );

        if ( login.Password.Hash() != user.Password )
            return NotFound( "Incorrect password" );
        
        var secret = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( "fake ass key but apparently it needs to be a lot longer for it to work" ) );
        var credentials = new SigningCredentials( secret, SecurityAlgorithms.HmacSha256 );
        
        // TODO Use ClaimTypes.Roles for admins n shit i guess
        
        var claims = new List<Claim>
        {
            new( "Id", $"{user.Id}" )
        };
             
        var options = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            claims: claims,
            expires: DateTime.Now.AddSeconds( 20 ),
            signingCredentials: credentials
        );
        
        string token = new JwtSecurityTokenHandler().WriteToken( options );
        return Ok( new { Token = token } ); 
    }

    [HttpPost, Route( "signup/{invite}" )]
    public async Task<IActionResult> Create( [FromBody] UserDetails details, string invite )
    {
        if ( !ModelState.IsValid )
            return BadRequest( ModelState );

        if ( details.FirstName is null || details.LastName is null || details.Email is null ||
             details.Password is null || details.PhoneNumber is null )
            return BadRequest();

        var family = Database.Families.FirstOrDefault( x => x.InviteCode == invite );

        if ( family is null )
            return Unauthorized( "Invalid Family Invite Code" );

        var user = new User
        {
            PhoneNumber = details.PhoneNumber,
            FirstName = details.FirstName,
            LastName = details.LastName,
            Email = details.Email,
            Password = details.Password.Hash(),
            Family = family
        };
        
        user.Created();

        Database.Users.Add( user );
        await Database.SaveChangesAsync();

        return Ok( user );
    }
}

public class LoginDetails
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string Password { get; set; }
}