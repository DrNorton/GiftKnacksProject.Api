using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Comments;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface ICommentRepository
    {
        Task<CommentDto> AddCommentToWish(long wishId,long commentUserId,string text, long? parentId = null);
        Task<CommentDto> AddCommentToGift(long giftId, long commentUserId, string text, long? parentId = null);
        Task<List<CommentDto>> GetCommentListByWishId(GetCommentsDto wishId);
        Task<List<CommentDto>> GetCommentListByGiftId(GetCommentsDto giftId);
        Task<long> GetOwnerWish(long wishId);
        Task<long> GetOwnerGift(long giftId);
        Task<long> GetAutorUserIdByCommentId(long parentCommentId);
        Task<List<CommentDto>> GetWishCommentsOlderById(GetCommentDto getCommentDto);

        Task<List<CommentDto>> GetGiftCommentsOlderById(GetCommentDto getCommentDto);
    }
}