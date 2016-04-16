using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Swashbuckle.Application;
using Swashbuckle.Swagger;

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
                var applyApiKeySecurity = new ApplyApiKeySecurity(
                   key: "ServiceBusToken",
                   name: "Authorization",
                   description: "Впиши сюда токен",
                   @in: "header"
                );
                applyApiKeySecurity.Apply(d);
            }).EnableSwaggerUi();
         
        }

        protected static string GetXmlCommentsPathForControllers()
        {
#if !DEBUG
            return System.String.Format(@"{0}bin\Documentation\GiftKnacksProject.Api.Controllers.XML", System.AppDomain.CurrentDomain.BaseDirectory);
#else
            return System.String.Format(@"{0}bin\GiftKnacksProject.Api.Controllers.XML", System.AppDomain.CurrentDomain.BaseDirectory);
#endif
        }

        protected static string GetXmlCommentsPathForModels()
        {
#if !DEBUG
            return System.String.Format(@"{0}bin\Documentation\GiftKnacksProject.Api.Dto.XML", System.AppDomain.CurrentDomain.BaseDirectory);
#else
            return System.String.Format(@"{0}bin\GiftKnacksProject.Api.Dto.XML", System.AppDomain.CurrentDomain.BaseDirectory);
#endif
        }
    }

    public class ApplyApiKeySecurity : IDocumentFilter, IOperationFilter
    {
        public ApplyApiKeySecurity(string key, string name, string description, string @in)
        {
            Key = key;
            Name = name;
            Description = description;
            In = @in;
        }

        public string Description { get; private set; }

        public string In { get; private set; }

        public string Key { get; private set; }

        public string Name { get; private set; }

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, System.Web.Http.Description.IApiExplorer apiExplorer)
        {
            IList<IDictionary<string, IEnumerable<string>>> security = new List<IDictionary<string, IEnumerable<string>>>();
            security.Add(new Dictionary<string, IEnumerable<string>> {
            {Key, new string[0]}
        });

            swaggerDoc.security = security;
        }

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, System.Web.Http.Description.ApiDescription apiDescription)
        {
            operation.parameters = operation.parameters ?? new List<Parameter>();
            operation.parameters.Add(new Parameter
            {
                name = Name,
                description = Description,
                @in = In,
                required = true,
                type = "string"
            });
        }

        public void Apply(Swashbuckle.Application.SwaggerDocsConfig c)
        {
            c.ApiKey(Key)
                .Name(Name)
                .Description(Description)
                .In(In);
            c.DocumentFilter(() => this);
            c.OperationFilter(() => this);
        }
    }
}