using HomeManagement.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeManagement.Server.Controllers.VersionOne;

public abstract class BaseVersionOneController : ControllerBase
{
    protected DatabaseContext Database { get; set; }

    /// <summary>
    /// Returns the current logged in user, or null
    /// </summary>
    /// <returns></returns>
    protected User? CurrentUser
    {
        get
        {
            string? id = HttpContext.User.Claims.FirstOrDefault( x => x.Type == "Id" )?.Value;
            return id is null ? null : Database.Users.FirstOrDefault( x => x.Id == Guid.Parse( id ) );
        }
    }
    
    protected BaseVersionOneController( DatabaseContext ctx )
    {
        Database = ctx;
    }
}