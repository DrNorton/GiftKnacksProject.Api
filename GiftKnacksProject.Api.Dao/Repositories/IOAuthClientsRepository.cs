using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos.OauthClients;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface IOAuthClientsRepository
    {
        Task<List<OAuthClientDto>> GetClients();
        OAuthClientDto FindClient(string clientId);
    }
}