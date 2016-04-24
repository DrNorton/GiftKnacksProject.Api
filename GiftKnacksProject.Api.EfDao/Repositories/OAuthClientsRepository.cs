using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.OauthClients;
using GiftKnacksProject.Api.EfDao.Base;

namespace GiftKnacksProject.Api.EfDao.Repositories
{
    public class OAuthClientsRepository: GenericRepository<OAuthClient>, IOAuthClientsRepository
    {
        public OAuthClientsRepository(EfContext db)
            : base(db)
        {

        }

        public async Task<List<OAuthClientDto>> GetClients()
        {
            var clients = await Db.Set<OAuthClient>().ToListAsync();
            return clients.Select(x=>new OAuthClientDto(){Active = x.Active,AllowedOrigin = x.AllowedOrigin,ApplicationType = x.ApplicationType,Id = x.Id,Name = x.Name,RefreshTokenLifeTime = x.RefreshTokenLifeTime,Secret = x.Secret}).ToList();
        }

        public  OAuthClientDto FindClient(string clientId)
        {
            var client =  Db.Set<OAuthClient>().Find(clientId);
            return new OAuthClientDto()
            {
                Active = client.Active,
                AllowedOrigin = client.AllowedOrigin,
                ApplicationType = client.ApplicationType,
                Id = client.Id,
                Name = client.Name,
                RefreshTokenLifeTime = client.RefreshTokenLifeTime,
                Secret = client.Secret
            };
        }
    }
}
