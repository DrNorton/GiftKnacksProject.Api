using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using GiftKnacksProject.Api.Controllers.ApiResults;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Dto.Dtos.Interesting;
using Microsoft.AspNet.Identity;

namespace GiftKnacksProject.Api.Controllers.Controllers
{
    [System.Web.Http.RoutePrefix("api/interestingnear")]
    [EnableCors(origins: "http://giftknackapi.azurewebsites.net", headers: "*", methods: "*")]
    public class InterestingController : CustomApiController
    {
        private readonly IProfileRepository _repository;
        private readonly IWishRepository _wishRepository;
        private readonly IGiftRepository _giftRepository;

        public InterestingController(IProfileRepository repository,IWishRepository wishRepository,IGiftRepository giftRepository)
        {
            _repository = repository;
            _wishRepository = wishRepository;
            _giftRepository = giftRepository;
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("Near")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> GetNear()
        {
            var userId = long.Parse(User.Identity.GetUserId());
            var userProfile=await _repository.GetProfile(userId);
            if (userProfile == null)
            {
                return ErrorApiResult(1000, "Профиль не найден");
            }

            var wishesNear=await _wishRepository.GetByArea(userProfile.Country, userProfile.City);
            var giftsNear =await _giftRepository.GetByArea(userProfile.Country, userProfile.City);
            var profilesNear = await _repository.GetByArea(userProfile.Country, userProfile.City);
            return SuccessApiResult(new InterestingNearDto() {
                Gifts = giftsNear.ToList(),
                Users = profilesNear.ToList(),
                Wishes = wishesNear.ToList()});
        }
    }
}
