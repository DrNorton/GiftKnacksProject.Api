using System.Threading.Tasks;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface ILinkRepository
    {
        Task<long> LinkWithGift(long userId, long wishId, long giftId);
        Task Unlink(long userId, long wishId, long giftId);
    }
}