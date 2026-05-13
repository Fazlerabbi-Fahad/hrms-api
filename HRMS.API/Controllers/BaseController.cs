using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            var claim = User.FindFirst("uid")?.Value;
            return claim != null ? int.Parse(claim) : 0;
        }

        protected string GetCurrentUserName()
        {
            return User.FindFirst("username")?.Value ?? string.Empty;
        }

        protected List<string> GetCurrentUserRoles()
        {
            return User.FindAll("roles").Select(c => c.Value).ToList();
        }
    }
}