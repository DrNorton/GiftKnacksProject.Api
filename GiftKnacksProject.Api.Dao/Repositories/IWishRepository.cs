using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Dto.Dtos.Interesting;
using GiftKnacksProject.Api.Dto.Dtos.Links;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface IWishRepository
    {
        Task<EmptyWishDto> GetEmptyDtoWithAdditionalInfo(long userId);
        Task<long> AddWish(long userId, WishDto wish);
        Task<IEnumerable<WishDto>> GetWishes(FilterDto filter);
        Task<WishDto> GetWish(long id);

        Task<IEnumerable<NearEntityDto>> GetByArea(CountryDto country,string city);
        Task CloseWish(long wishId, long currentUserId,long? closerId);
        Task<BasicWishGiftDto> GetBasicInfo(long l);
        Task<List<ParticipantDto>> GetAllParticipants(long closedItemId);
        Task<WishDto> UpdateWish(long userId, WishDto updatedWish);
        Task<List<WishCategoryDto>> GetWishCategories();
    }
}