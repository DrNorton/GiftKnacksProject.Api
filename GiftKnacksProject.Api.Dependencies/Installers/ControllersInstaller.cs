using System.Web.Http;
using System.Web.Http.Filters;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GiftKnacksProject.Api.Controllers.ApiResults;

namespace GiftKnacksProject.Api.Dependencies.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<ApiResult>()
                    .BasedOn<ApiController>()
                    .LifestyleTransient());
            container.Register(Component.For<IFilter, ExceptionHandler>().LifestyleTransient());

        }

        public class ExceptionHandler : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext context)
            {
                var result= new ApiResult(context.Request, 1000, context.Exception.Message.ToString(), null);
                var ex=result.Execute();
                context.Response = ex;
            }
        }
    }
}
