using System;

namespace DatingApp.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = null) : base(message ?? string.Empty)
        {
        }
    }
}