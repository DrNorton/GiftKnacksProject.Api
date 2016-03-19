using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos;
using GiftKnacksProject.Api.EfDao.Base;

namespace GiftKnacksProject.Api.EfDao.Repositories
{
    public class CountryRepository:GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(EfContext context)
            : base(context)
        {
          
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountries()
        {
            return Db.Set<Country>().ToList().Select(x => new CountryDto() {Code = x.Id, Name = x.Name});
        }
    }
}
