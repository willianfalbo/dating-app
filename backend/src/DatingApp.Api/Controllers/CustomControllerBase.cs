using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Api.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        // check if the user id parameter matches with the token id
        // it is needed in case of cheating user id
        protected bool DoesUserMatchWithToken(int id)
        {
            if (id != GetUserId())
                return false;
            else
                return true;
        }

        protected int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}