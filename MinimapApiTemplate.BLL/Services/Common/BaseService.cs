using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MinimalApiTemplate.DAL;

namespace MinimapApiTemplate.BLL.Services.Common
{
    public abstract class BaseService
    {
        protected GenericContext DataContext { get; }

        protected ILogger Logger { get; }

        public BaseService(GenericContext dataContext, ILogger logger)
        {
            DataContext = dataContext;
            Logger = logger;
        }
    }
}
