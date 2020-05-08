using System.Threading.Tasks;

namespace Application.Interfaces
{
    /// <summary>
    /// Interface to create HTML document based on RazorView
    /// </summary>
    public interface IRazorViewToStringRenderer
    {
        /// <summary>
        /// Create HTML document.
        /// </summary>
        /// <typeparam name="TModel">Generalized model.</typeparam>
        /// <param name="viewName">Name of Razor view.</param>
        /// <param name="model">View model.</param>
        /// <returns>HTML document as a string.</returns>
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}