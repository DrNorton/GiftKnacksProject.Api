using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos;
using GiftKnacksProject.Api.Dto.Dtos.Interesting;
using GiftKnacksProject.Api.Dto.Dtos.Profile;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface IProfileRepository
    {
        Task<ProfileDto> GetProfile(long userId);
        Task UpdateProfile(ProfileDto profile);
        Task<ShortProfileDto> GetShortProfile(long userId);
        Task<IEnumerable<NearEntityDto>> GetByArea(CountryDto country, string city);
        void UpdateLastLoginTime(long userId,DateTime time);
        Task<List<TinyProfileDto>> Search(string pattern);
        Task<bool> CheckActivity(long userId);
        Task<TinyProfileDto> GetTinyProfile(long id);
        Task<List<TinyProfileDto>> GetTinyProfiles(IEnumerable<long> usersIds);
    }
}