using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.AuthUsers;
using GiftKnacksProject.Api.Dto.AuthUsers.External;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface IExternalLoginRepository
    {
        Task<ExternalUserDto> FindAsync(string loginProvider, string providerKey);
        Task AddLogin(ApplicationUser user, UserLoginInfo login);
    }
}
