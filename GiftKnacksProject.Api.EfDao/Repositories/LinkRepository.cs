using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.EfDao.Base;
using GiftKnacksProject.Api.Helpers;

namespace GiftKnacksProject.Api.EfDao.Repositories
{
    public class LinkRepository : GenericRepository<WishGiftLink>, ILinkRepository
    {
        public LinkRepository(EfContext context)
            :base(context)
        {
            
        }


        public async  Task<long> LinkWithGift(long userId, long wishId, long giftId)
        {
            var finded = Db
                .Set<WishGiftLink>()
                .FirstOrDefault(x => x.GiftId == giftId && x.WishId == wishId && x.UserId == userId);
            if (finded != null)
            {
                throw new ExceptionWithCode(109,"link already exists");
            }
            var newLink =  Db.Set<WishGiftLink>().Create();
            newLink.CreatedTime = DateTime.Now;
            newLink.GiftId = giftId;
            newLink.WishId = wishId;
            newLink.UserId = userId;
            base.Insert(newLink);
            base.Save();
            return newLink.Wish.UserId;
        }

        public Task Unlink(long userId, long wishId, long giftId)
        {
            var newLink = Db.Set<WishGiftLink>().FirstOrDefault(x => x.WishId == wishId && x.GiftId == giftId && x.UserId==userId);
            if (newLink == null)
            {
                throw new Exception("link not finded");
            }
            Db.Set<WishGiftLink>().Remove(newLink);
            return Db.SaveChangesAsync();
        }

    }
}
