using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos;
using GiftKnacksProject.Api.Dto.Dtos.Gifts;
using GiftKnacksProject.Api.Dto.Dtos.Interesting;
using GiftKnacksProject.Api.Dto.Dtos.Links;

using GiftKnacksProject.Api.Dto.Dtos.Wishes;
using GiftKnacksProject.Api.EfDao.Base;

namespace GiftKnacksProject.Api.EfDao.Repositories
{
    public class WishRepository:GenericRepository<Wish>, IWishRepository
    {
         public WishRepository(EfContext context)
            : base(context)
        {

        }
      
        //Получение пустого виша
        public async Task<EmptyWishDto> GetEmptyDtoWithAdditionalInfo(long userId)
        {
            var profile = Db.Set<Profile>().FirstOrDefault(x=>x.Id==userId);
            var wishCategories = Db.Set<WishCategory>().Where(x=>! x.WishCategories1.Any()).Select(x=>new WishCategoryDto(){Description = x.Description,Name = x.Name,ParentName=x.WishCategory1.Name}).ToList();
            var country = profile.Country == null
                ? null
                : new CountryDto()
                {
                    Code = profile.Country1.Id,
                    Name = profile.Country1.Name
                };

            return new EmptyWishDto(){Country = country ,
                WishCategories = wishCategories,
                FromDate = DateTime.Now,
                City = profile.City};
        }
        
        //Добавление вишеа
        public async Task<long> AddWish(long userId,WishDto wish)
        {
            var newWish=new Wish();
            newWish=UpdateEfModelFromDto(newWish, wish, userId);
            base.Insert(newWish);
            base.Save();
            return newWish.Id;
        }

        //Получение вишей
        public async Task<IEnumerable<WishDto>> GetWishes(Dto.Dtos.Gifts.FilterDto filter)
        {
            IQueryable<Wish> query = Db.Set<Wish>().AsQueryable();
            if (filter != null)
            {
                if (filter.StatusCode != -1)
                {
                    query = query.Where(x => x.GiftWishStatus.Code == filter.StatusCode);
                }

                if (filter.UserId != null)
                { 
                    query = query.Where(x => x.UserId == filter.UserId);
                }

                if (!String.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(filter.Keyword));
                }

                if (filter.Country != null && filter.Country.Name!=null)
                {
                    query = query.Where(x => x.Country1.Name == filter.Country.Name);
                }
                if (!String.IsNullOrEmpty(filter.City))
                {
                    query = query.Where(x => x.Country1.Name.Contains(filter.City));
                }

                if (!(filter.From == null && filter.To == null))
                {
                    query.Where(x => (x.FromDate <= filter.To) && (x.FromDate >= filter.From));
                }
            }


            if (filter.Length != 0)
            {
                query = query.OrderByDescending(x => x.User.AvgRate).Skip(filter.Offset).Take(filter.Length);
            }
            else
            {
                query = query.OrderByDescending(x => x.User.AvgRate);
            }

            return query.Select(x => new WishDto()
            {
                Country = new CountryDto() { Code = x.Country1.Id, Name = x.Country1.Name },
                City = x.City,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                Name = x.Name,
                Id = x.Id,
                Status = new StatusDto() { Code = x.GiftWishStatus.Code, Status = x.GiftWishStatus.Status },
                Creator = new CreatorDto() { AvatarUrl = x.User.Profile.AvatarUrl, CreatorId = x.User.Id, FirstName = x.User.Profile.FirstName, LastName = x.User.Profile.LastName }
            }).ToList();
        }

        //Получение конкретного виша
        public Task<WishDto> GetWish(long id)
        {
            var wish = Db.Set<Wish>().Find(id);
            if (wish != null)
            {
                var dto = ConvertToDto(wish);

                return Task.FromResult(dto);
            }
            else
            {
                return Task.FromResult(default(WishDto));
            }

        }

        private static WishDto ConvertToDto(Wish wish)
        {
            var dto = new WishDto()
            {
                Id = wish.Id,
                Country = new CountryDto() {Code = wish.Country1.Id, Name = wish.Country1.Name},
                Description = wish.Description,
                Benefit = wish.Benefit,
                ImageUrl = wish.ImageUrl,
                City = wish.City,
                FromDate = wish.FromDate,
                ToDate = wish.ToDate,
                Location = wish.Location,
                Name = wish.Name,
                Status = new StatusDto() {Code = wish.GiftWishStatus.Code, Status = wish.GiftWishStatus.Status},
                Emergency = wish.Emergency,
                Category = wish.WishCategory.Name,
                Participants =
                    wish.WishGiftLinks.Select(
                        x =>
                            new ParticipantDto()
                            {
                                FirstName = x.Gift.User.Profile.FirstName,
                                Id = x.Gift.User.Id,
                                LastName = x.Gift.User.Profile.LastName
                            }),
                Creator =
                    new CreatorDto()
                    {
                        AvatarUrl = wish.User.Profile.AvatarUrl,
                        CreatorId = wish.User.Id,
                        FirstName = wish.User.Profile.FirstName,
                        LastName = wish.User.Profile.LastName,
                        FavoriteContact =
                            wish.User.Profile.Contacts.Where(x => x.MainContact)
                                .Select(
                                    x =>
                                        new ContactDto()
                                        {
                                            Name = x.ContactType.Name,
                                            Value = x.Value,
                                            MainContact = x.MainContact
                                        })
                                .FirstOrDefault()
                    },
            };
            return dto;
        }

        public async Task CloseWish(long wishId,long currentUserId,long? closerId)
        {
            var wish = await Db.Set<Wish>().FindAsync(wishId);
            if (closerId != null)
            {
                wish.WishUserCloserId = closerId;
            }
            if (wish.User.Id != currentUserId)
            {
                throw new Exception("Error. You try close thing, but you are not creator");
            }
            wish.GiftWishStatus = Db.Set<GiftWishStatus>().FirstOrDefault(x => x.Code == 1);
            base.Update(wish);
            base.Save();
        }

        public Task<BasicWishGiftDto> GetBasicInfo(long id)
        {
            return Db.Set<Wish>().Where(x=>x.Id==id).Select(x=>new BasicWishGiftDto() {Id = x.Id,Title = x.Name,Owner = x.UserId,ImageUrl=x.ImageUrl}).FirstOrDefaultAsync();
        }

        public Task<List<ParticipantDto>> GetAllParticipants(long closedItemId)
        {
            return Db.Set<WishGiftLink>().Where(x => x.WishId == closedItemId).Select(x => new ParticipantDto() { Id = x.Gift.UserId }).ToListAsync();
        }

        public async Task<WishDto> UpdateWish(long userId, WishDto updatedWish)
        {
            var originalWish = Db.Set<Wish>().Find(updatedWish.Id);
            originalWish = UpdateEfModelFromDto(originalWish, updatedWish, userId);
            base.Update(originalWish);
            await Db.SaveChangesAsync();
            return ConvertToDto(originalWish);
        }

        private Wish UpdateEfModelFromDto(Wish wish, WishDto dto, long userId)
        {
            var category = Db.Set<WishCategory>().FirstOrDefault(x => x.Name == dto.Category);
            var country = Db.Set<Country>().FirstOrDefault(x => x.Id == dto.Country.Code);
            var status = Db.Set<GiftWishStatus>().FirstOrDefault(x => x.Code.Equals(0));
            wish.Benefit = dto.Benefit;
            wish.WishCategory = category;
            wish.City = dto.City;
            wish.Country1 = country;
            wish.Description = dto.Description;
            wish.FromDate = dto.FromDate;
            wish.ToDate = dto.ToDate;
            wish.ImageUrl = dto.ImageUrl;
            wish.UserId = userId;
            wish.Emergency = dto.Emergency;
            wish.Name = dto.Name;
            wish.Location = dto.Location;
            wish.GiftWishStatus = status;
            return wish;
        }

        //Получение виша по городу округу
        public async Task<IEnumerable<NearEntityDto>> GetByArea(CountryDto country,string city)
        {
            IQueryable<Wish> query = Db.Set<Wish>().AsQueryable();
            if (country != null && country.Name != null && city != null)
            {
                 query=query.Where(x=>x.Country1.Name==country.Name || x.City==city);
            }


            return query.Select(x => new NearWishGiftDto() {Id = x.Id, Name = x.Name});

        } 

    }
}
