using System;

namespace DatingApp.Core.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message = null) : base(message ?? string.Empty)
        {
        }
    }
}