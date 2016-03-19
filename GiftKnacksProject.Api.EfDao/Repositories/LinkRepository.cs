using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.EfDao.Base;

namespace GiftKnacksProject.Api.EfDao.Repositories
{
    public class LinkRepository : GenericRepository<WishGiftLink>, ILinkRepository
    {
        public LinkRepository(EfContext context)
            :base(context)
        {
            
        }


        public async Task<long> LinkWithGift(long userId, long wishId, long giftId)
        {
            var newLink =  Db.Set<WishGiftLink>().Create();
            newLink.CreatedTime = DateTime.Now;
            newLink.GiftId = giftId;
            newLink.WishId = wishId;
            newLink.UserId = userId;
            base.Insert(newLink);
            base.Save();
            return newLink.Wish.UserId;
        }
    }
}
