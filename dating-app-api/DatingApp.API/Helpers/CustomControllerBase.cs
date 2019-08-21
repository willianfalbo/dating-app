using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Helpers
{
    public class CustomControllerBase : ControllerBase
    {
        // check if the user id parameter matches with the token id
        // it is needed in case of cheating user id
        protected bool DoesUserMatchWithToken(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return false;
            else
                return true;
        }
    }
}