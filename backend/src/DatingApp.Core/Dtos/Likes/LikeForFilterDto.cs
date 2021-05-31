namespace DatingApp.Core.Dtos.Likes
{
    public class LikeForFilterDto
    {
        private const int PageSizeLimit = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > PageSizeLimit) ? PageSizeLimit : value; }
        }
        public bool Senders { get; set; } = true;
    }
}