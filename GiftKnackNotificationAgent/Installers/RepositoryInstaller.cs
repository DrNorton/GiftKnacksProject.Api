using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GiftKnacksProject.Api.Dao.Repositories;
using GiftKnacksProject.Api.EfDao;
using GiftKnacksProject.Api.EfDao.Repositories;

namespace GiftKnackNotificationAgent.Installers
{
    public class RepositoryInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<EfContext>().LifestyleTransient());

            container.Register(Component.For<IAuthRepository, EfAuthRepository>().LifestyleTransient());

            container.Register(Component.For<IProfileRepository, ProfileRepository>().LifestyleTransient());

            container.Register(Component.For<ICountryRepository, CountryRepository>().LifestyleTransient());
            container.Register(Component.For<IWishRepository, WishRepository>().LifestyleTransient());
            container.Register(Component.For<IGiftRepository, GiftRepository>().LifestyleTransient());
            container.Register(Component.For<ILinkRepository, LinkRepository>().LifestyleTransient());
            container.Register(Component.For<IReferenceRepository, ReferenceRepository>().LifestyleTransient());
            container.Register(Component.For<ICommentRepository, CommentRepository>().LifestyleTransient());
        }
    }
}
