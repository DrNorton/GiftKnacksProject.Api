using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GiftKnackProject.NotificationTypes;
using GiftKnackProject.NotificationTypes.ProcessedNotifications;
using GiftKnackProject.NotificationTypes.ProcessedNotifications.Infos;
using GiftKnacksProject.Api.Dao.Repositories;

namespace GiftKnackNotificationAgent.Models.Handlers
{
    public class AddReferenceNotificationHandler : BaseNotificationHandler<AddReferenceQueueNotification>
    {
        private readonly IReferenceRepository _referenceRepository;

        public AddReferenceNotificationHandler(IReferenceRepository referenceRepository)
        {
            _referenceRepository = referenceRepository;
        }

        public override bool IsMultipleNotification => false;
        public override async Task<BaseNotificationInfo> ProcessInputMessageAndLoadAdditionalInfoForSingleNotification(AddReferenceQueueNotification messageFromQueue)
        {
            var referenceId = messageFromQueue.RefefenceId;
            var reference = await _referenceRepository.GetById(referenceId);
            return new AddReferenceInfo() { OwnerId = (long)reference.OwnerId, User = reference.Replyer, Rate = reference.Rate,TargetUserId=(long)reference.OwnerId };
        }

        public override Task<IEnumerable<BaseNotificationInfo>> ProcessInputMessageAndLoadAdditionalInfoForMultipleNotification(AddReferenceQueueNotification messageFromQueue)
        {
            throw new NotImplementedException();
        }
    }
}
