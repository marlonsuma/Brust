using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using NiQ_Donor_Tracking_System;
using Swashbuckle.Swagger;
using System.Web.Http.Description;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace NiQ_Donor_Tracking_System
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                               .EnableSwagger(c =>
                                   {
                                       c.SingleApiVersion("v1", "NiQ Donor Kit API")
                                        .Description("NiQ Donor Kit Tracking by Computype.")
                                        .Contact(cc =>
                                            cc.Name("Computype")
                                              .Email("contactus@computype.com")
                                              .Url("https://www.computype.com"))
                                        .License(l => l.Name("Computype, Inc. Standard Purchase Terms and Conditions")
                                                       .Url("https://www.computype.com/terms-and-conditions"));

                                       //c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();

                                       c.SchemaId(SchemaIdStrategy);
                                       
                                       var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                                       var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                                       var commentsFile = Path.Combine(baseDirectory + @"\bin", commentsFileName);
                                       c.IncludeXmlComments(commentsFile);
                                       c.PrettyPrint();
                                   })
                               .EnableSwaggerUi();
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.UseXmlSerializer = true;
            
        }

        private static string SchemaIdStrategy(Type currentClass)
        {
            return currentClass.Name.Replace("Model", string.Empty);
        }


        public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
        {
            public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
            {
                if (operation.parameters == null)
                {
                    operation.parameters = new List<Parameter>();
                }
                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "access token",
                    required = false,
                    type = "string"
                });
            }
        }
    }
}
