namespace DatingApp.Core.Dtos.Messages
{
    public class MessageForFilterDto
    {
        private const int _maxLimit = 50;
        public int Page { get; set; } = 1;
        private int _limit = 10;
        public int Limit
        {
            get { return _limit; }
            set { _limit = (value > _maxLimit) ? _maxLimit : value; }
        }

        public string Container { get; set; } = "unread";
    }
}