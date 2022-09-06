using Microsoft.AspNetCore.Builder;
using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWPurchaseCore5API.Configuration
{
    public static class OpenApiSpecificationExtensions
    {
		/// <summary>
		/// Use Open API Specification (Swagger) middlewares.
		/// </summary>
		/// <param name="app">Application pipeline builder.</param>
		public static void UseOpenApiSpecification(this IApplicationBuilder app)
		{		

			app.UseOpenApi(settings =>
			{
				settings.PostProcess = (document, request) =>
				{
					document.Info.Version = "v1";
					document.Info.Title = "AW Purchase API";
					document.Info.Description = "AccountantsWorld Purchase API";
					document.Info.TermsOfService = "https://www.accountantsworld.com/Terms.html";
					document.Info.Contact = new OpenApiContact()
					{
						Name = "AccountantsWorld",
						Email = "support@accountantsworld.com",
						Url = "https://accountantsworld.com"
					};
				};
			});
			app.UseSwaggerUi3(c =>
			{
				c.DocExpansion = "list";
				c.DefaultModelsExpandDepth = -1;
				c.DocumentPath = "/swagger/v1/swagger.json";
			});
		}
	}
}
