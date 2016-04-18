using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.Dto.Dtos.Profile;
using GiftKnacksProject.Api.Dto.Dtos.Reference;
using GiftKnacksProject.Api.EfDao.Base;

namespace GiftKnacksProject.Api.EfDao.Repositories
{
    public class ReferenceRepository: GenericRepository<Reference>, IReferenceRepository
    {
        private readonly EfContext _context;

        public ReferenceRepository(EfContext context)
            :base(context)
        {
            _context = context;
        }

        public async Task<long> AddOrUpdateReference(long ownerId, long replyerId,byte rate,string text)
        {
            var findedReference = _context.Set<Reference>().FirstOrDefault(x => x.OwnerId == ownerId && x.ReplyerId == replyerId);
            long id;
            if (findedReference == null)
            {
                var newReference = new Reference()
                {
                    OwnerId = ownerId,
                    ReplyerId = replyerId,
                    Text = text,
                    Rate = rate,
                    CreatedTime = DateTime.Now
                };
                base.Insert(newReference);
                id = newReference.Id;
            }
            else
            {
                findedReference.Rate = rate;
                findedReference.CreatedTime=DateTime.Now;
                findedReference.Text = text;
                base.Update(findedReference);
                id = findedReference.Id;
            }
       
           
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<List<ReferenceDto>> GetByOwnerId(long ownerId)
        {
          
            return (await
                _context.Set<Reference>()
                    .Where(x => x.OwnerId == ownerId).OrderByDescending(x=>x.CreatedTime).ToListAsync())
                .Select(
                    x =>
                        new ReferenceDto()
                        {
                            OwnerId = x.OwnerId,
                            Rate = x.Rate,
                            CreatedTime = x.CreatedTime,
                            ReferenceText = x.Text,
                            Replyer =
                                new TinyProfileDto()
                                {
                                    Id = x.User1.Id,
                                    AvatarUrl = x.User1.Profile.AvatarUrl,
                                    FirstName = x.User1.Profile.FirstName,
                                    LastName = x.User1.Profile.LastName,
                                    AvgRate = x.User1.AvgRate,
                                    TotalClosed = x.User1.TotalClosed
                                }
                        }).ToList();

        }

        public Task<ReferenceDto> GetById(long referenceId)
        {
           
                return _context.Set<Reference>()
                    .Where(x => x.Id == referenceId).Select(
                    x =>
                        new ReferenceDto()
                        {
                            OwnerId = x.OwnerId,
                            Rate = x.Rate,
                            ReferenceText = x.Text,
                            Replyer =
                                new TinyProfileDto()
                                {
                                    Id = x.User1.Id,
                                    AvatarUrl = x.User1.Profile.AvatarUrl,
                                    FirstName = x.User1.Profile.FirstName,
                                    LastName = x.User1.Profile.LastName,
                                    AvgRate = x.User.AvgRate,
                                    TotalClosed = x.User.TotalClosed
                                }
                        }).FirstOrDefaultAsync();
        }

   
    }
}
