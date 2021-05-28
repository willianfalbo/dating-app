using System;

namespace DatingApp.Core.Helpers
{
    public static class Extensions
    {
        public static int CalculateAge(this DateTimeOffset dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}
