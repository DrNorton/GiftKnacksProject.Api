using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.AuthUsers;
using GiftKnacksProject.Api.Dto.AuthUsers.External;
using GiftKnacksProject.Api.EfDao.Base;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.EfDao.Repositories
{
    public class ExternalLoginRepository: GenericRepository<ExternalLogin>, IExternalLoginRepository
    {
        public ExternalLoginRepository(EfContext context)
            : base(context)
        {

        }

        public async Task<ExternalUserDto> FindAsync(string loginProvider, string providerKey)
        {
            var externalLogin = await Db
                .Set<ExternalLogin>()
                .Where(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey)
                .Select(
                    x =>
                        new ExternalUserDto()
                        {
                            ExternalLoginId = x.ExternalLoginId,
                            LoginProvider = x.LoginProvider,
                            ProviderKey = x.ProviderKey,
                            User =
                                new ApplicationUser()
                                {
                                    ConfirmEmail = x.User.ConfirmMail,
                                    EmailStamp = x.User.EmailStamp,
                                    Id = x.User.Id,
                                    PasswordHash = x.User.Password,
                                    UserName = x.User.Email
                                }
                        })
                .ToListAsync();
            if (externalLogin.Any())
            {
                return externalLogin.FirstOrDefault();
            }
            else
            {
                return null;
            }

        }

        public Task AddLogin(ApplicationUser user, UserLoginInfo login)
        {
            Db.Set<ExternalLogin>()
                .Add(new ExternalLogin()
                {
                    UserId = user.Id,
                    ExternalLoginId = Guid.NewGuid(),
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey
                });
            return Db.SaveChangesAsync();
        }
    }
}
