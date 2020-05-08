using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// Implementation is taken from:
// https://github.com/aspnet/Entropy/tree/master/samples/Mvc.RenderViewToString

namespace Infrastructure.Services
{
    /// <summary>
    /// Render to form HTML document basen on Razor View.
    /// </summary>
    public class RazorViewToStringRenderer : IRazorViewToStringRenderer
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="viewEngine">Razor view engine.</param>
        /// <param name="tempDataProvider">Temp data provider.</param>
        /// <param name="serviceProvider">Service provider.</param>
        public RazorViewToStringRenderer(IRazorViewEngine viewEngine,
                                         ITempDataProvider tempDataProvider,
                                         IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
            _tempDataProvider = tempDataProvider ?? throw new ArgumentNullException(nameof(tempDataProvider));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc />
        public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
        {
            var actionContext = GetActionContext();
            var view = FindView(actionContext, viewName);

            using var output = new StringWriter();

            ViewContext viewContext =
                new ViewContext(actionContext, view,
                                new ViewDataDictionary<TModel>(
                                    metadataProvider: new EmptyModelMetadataProvider(),
                                    modelState: new ModelStateDictionary())
                                {
                                    Model = model
                                },
                                new TempDataDictionary(
                                    actionContext.HttpContext,
                                    _tempDataProvider),
                                output,
                                new HtmlHelperOptions());

            await view.RenderAsync(viewContext);

            return output.ToString();
        }

        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[]
                {
                    $"Unable to find view '{viewName}'. The following locations were searched:"
                }
                .Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };

            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}