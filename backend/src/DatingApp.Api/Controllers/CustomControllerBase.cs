using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        protected int GetUserIdFromToken()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}