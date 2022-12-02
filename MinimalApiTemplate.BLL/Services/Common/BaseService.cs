using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MinimalApiTemplate.DAL;

namespace MinimapApiTemplate.BLL.Services.Common
{
    public abstract class BaseService
    {
        protected IMapper mapper;

        protected GenericContext DataContext { get; }

        protected ILogger Logger { get; }

        public BaseService(GenericContext dataContext, ILogger logger, IMapper mapper)
        {
            DataContext = dataContext;
            Logger = logger;
            this.mapper = mapper;
        }
    }
}
