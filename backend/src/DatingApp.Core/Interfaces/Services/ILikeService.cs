using System.Threading.Tasks;

namespace DatingApp.Core.Interfaces.Services
{
    public interface ILikeService
    {
        Task LikeUser(int id, int recipientId);
    }
}