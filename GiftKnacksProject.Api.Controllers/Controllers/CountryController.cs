using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.EfDao.Base;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/Country")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CountryController : CustomApiController
    {
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        //[System.Web.Http.Authorize]
        //[System.Web.Http.Route("GetCountries")]
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> GetCountries()
        {
            var countriesList=await _countryRepository.GetAllCountries();
            return SuccessApiResult(countriesList.ToList());
        }
    }


}
