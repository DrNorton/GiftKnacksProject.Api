using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using Swashbuckle.Application;

namespace GiftKnacksProject.Api.App_Start
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSwagger(d =>
            {
                d.SingleApiVersion("v1", "Проект обмена подарками API");
                d.IncludeXmlComments(GetXmlCommentsPathForControllers());
                d.IncludeXmlComments(GetXmlCommentsPathForModels());
                d.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                d.ApiKey("token")
                         .Description("API Token Authentication")
                         .Name("Bearer")
                         .In("header");
              
            }).EnableSwaggerUi();
        }

        protected static string GetXmlCommentsPathForControllers()
        {
#if RELEASE
            return System.String.Format(@"{0}bin\Documentation\GiftKnacksProject.Api.Controllers.XML", System.AppDomain.CurrentDomain.BaseDirectory);
#endif
#if DEBUG
            return System.String.Format(@"{0}bin\GiftKnacksProject.Api.Controllers.XML", System.AppDomain.CurrentDomain.BaseDirectory);
#endif
        }

        protected static string GetXmlCommentsPathForModels()
        {
#if RELEASE
             return System.String.Format(@"{0}bin\Documentation\GiftKnacksProject.Api.Dto.XML", System.AppDomain.CurrentDomain.BaseDirectory);
#endif

#if DEBUG
            return System.String.Format(@"{0}bin\GiftKnacksProject.Api.Dto.XML", System.AppDomain.CurrentDomain.BaseDirectory);
#endif
        }
    }
}