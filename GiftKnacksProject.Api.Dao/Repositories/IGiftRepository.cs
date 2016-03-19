using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Dto.Dtos.Interesting;
using GiftKnacksProject.Api.Dto.Dtos.Links;
using GiftKnacksProject.Api.Dto.Dtos.Wishes;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface IGiftRepository
    {
        Task<EmptyGiftDto> GetEmptyDtoWithAdditionalInfo();
        Task<long> AddGift(long userId, GiftDto gift);
        Task<IEnumerable<GiftDto>> GetGifts(FilterDto filter);
        Task<GiftDto> GetGift(long id);
        Task<IEnumerable<NearEntityDto>> GetByArea(CountryDto country, string city);
        Task CloseGift(long giftId, long currentUserId);
        Task<BasicWishGiftDto> GetBasicInfo(long targetId);
        Task<List<ParticipantDto>> GetAllParticipants(long closedItemId);
    }
}