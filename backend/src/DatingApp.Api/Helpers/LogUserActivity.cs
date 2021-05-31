using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Api.Helpers
{
    /// <summary>
    /// This is class is intended to update the user activity.
    /// </summary>
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            var _service = resultContext.HttpContext.RequestServices.GetService<IUsersService>();

            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _service.UpdateUserActivity(userId);
        }
    }
}