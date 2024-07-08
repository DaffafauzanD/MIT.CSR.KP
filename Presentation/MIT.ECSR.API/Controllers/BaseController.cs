using Microsoft.AspNetCore.Mvc;
using MIT.ECSR.Shared.Attributes;
using MediatR;
using MIT.ECSR.Shared.Interface;
using AutoMapper;
using System.Security.Claims;

namespace MIT.ECSR.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController<T> : Controller
    {
        private IMediator _mediatorInstance;
        private ILogger<T> _loggerInstance;
        private IWrapperHelper _wrapperInstance;
        private IMapper _mapperInstance;
        private IGeneralHelper _helperInstance;
        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();
        protected IWrapperHelper _wrapper => _wrapperInstance ??= HttpContext.RequestServices.GetService<IWrapperHelper>();
        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();
        protected IGeneralHelper _helper => _helperInstance ??= HttpContext.RequestServices.GetService<IGeneralHelper>();
        protected IActionResult Wrapper<TT>(TT val)
        {
            dynamic result = val!;
            int code = result.Code;
            return this.StatusCode(code, val);
        }

        protected TokenObject Token
        {
            get
            {
                var result = new TokenObject();
                if (HttpContext.Request.Headers.TryGetValue("Authorization", out var requestKey))
                {
                    if (requestKey.Count() > 0)
                    {
                        var key = requestKey.First().Split(' ');
                        if (key.Length == 2 && key[0].ToLower().Trim() == "bearer")
                        {
                            if(_helperInstance==null)
                                _helperInstance ??= HttpContext.RequestServices.GetService<IGeneralHelper>();
                            var decode = _helperInstance.DecodeToken(key[1]);
                            result = decode.Data;
                        }
                    }
                }
                return result;
            }
        }
    }

}
