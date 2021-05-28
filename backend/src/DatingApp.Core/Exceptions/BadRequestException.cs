using System;

namespace DatingApp.Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message = null) : base(message ?? string.Empty)
        {
        }
    }
}