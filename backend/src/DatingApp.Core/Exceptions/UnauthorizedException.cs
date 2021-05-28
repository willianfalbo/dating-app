using System;

namespace DatingApp.Core.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = null) : base(message ?? string.Empty)
        {
        }
    }
}