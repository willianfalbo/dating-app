namespace DatingApp.Core.Dtos.Users
{
    public class UserForFilterDto
    {
        private const int _maxLimit = 50;
        public int Page { get; set; } = 1;
        private int _limit = 10;
        public int Limit
        {
            get { return _limit; }
            set { _limit = (value > _maxLimit) ? _maxLimit : value; }
        }

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