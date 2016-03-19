using System.Collections.Generic;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dto.Dtos;

namespace GiftKnacksProject.Api.Dao.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<CountryDto>> GetAllCountries();
    }
}