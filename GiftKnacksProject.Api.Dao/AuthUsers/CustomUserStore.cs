using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.AuthUsers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GiftKnacksProject.Api.Dao.AuthUsers
{
    public class CustomUserStore : IUserStore<ApplicationUser,long>,IUserPasswordStore<ApplicationUser,long>,IUserEmailStore<ApplicationUser,long>,IUserSecurityStampStore<ApplicationUser,long>,IUserLoginStore<ApplicationUser,long>
    {
        private readonly IAuthRepository _repository;
        private readonly IExternalLoginRepository _externalLoginRepository;

        public CustomUserStore(IAuthRepository repository,IExternalLoginRepository externalLoginRepository)
        {
            _repository = repository;
            _externalLoginRepository = externalLoginRepository;
        }

        public Task CreateAsync(ApplicationUser user)
        {
            return _repository.RegisterUser(user);
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(long userId)
        {
            return _repository.FindUser(userId);
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return  _repository.FindUser(userName, "");
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return _repository.UpdateUser(user);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PasswordHash);

        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return FindByNameAsync(email);
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.UserName);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.FromResult(user.ConfirmEmail);
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.FromResult(user.ConfirmEmail=confirmed);
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return Task.FromResult(user.EmailStamp);
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            return Task.FromResult(user.EmailStamp=stamp);
        }

        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return _externalLoginRepository.AddLogin(user,login);
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            var externalUser=await _externalLoginRepository.FindAsync(login.LoginProvider,login.ProviderKey);
            return externalUser?.User;
        }
    }
}
