using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.Reference;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface IReferenceRepository
    {
        Task<long> AddReference(long ownerId, long replyerId, byte rate, string text);
        Task<List<ReferenceDto>> GetByOwnerId(long ownerId);
        Task<ReferenceDto> GetById(long referenceId);
    }
}