using System.Linq;

namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        private const int MAX_PAGE_SIZE = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MAX_PAGE_SIZE) ? MAX_PAGE_SIZE : value; }
        }

        public int UserId { get; set; }
        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                value = value?.ToLower()?.Trim();
                _gender = (value == "male" || value == "female") ? value : "unknown";
            }
        }

        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; }
    }
}