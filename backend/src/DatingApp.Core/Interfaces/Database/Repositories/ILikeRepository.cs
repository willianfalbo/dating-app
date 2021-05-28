using System.Threading.Tasks;
using DatingApp.Core.Entities;

namespace DatingApp.Core.Interfaces.Database.Repositories
{
    public interface ILikeRepository : IRepository<Like>
    {
        Task<Like> GetLike(int userId, int recipientId);
    }
}