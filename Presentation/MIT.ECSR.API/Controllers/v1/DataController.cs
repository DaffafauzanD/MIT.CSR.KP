using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using MIT.ECSR.Data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using MIT.ECSR.Shared.Interface;
using WonderKid.DAL.Interface;
using MIT.ECSR.Shared.Attributes;

namespace MIT.ECSR.API.Controllers
{
    public partial class DataController : BaseController<DataController>
    {
        private readonly IUnitOfWork<ApplicationDBContext> _context;
        public DataController(IUnitOfWork<ApplicationDBContext> context)
        {
            _context = context;
        }
        [HttpPost(template: "query")]
        public async Task<IActionResult> Query([FromBody] QueryRequest query)
        {
            var result = new ListResponse<Dictionary<string, string>>();
            var execute = await _context.DynamicQuery(query.Query);
            if (execute.Success)
            {
                result.List = execute.Result;
                result.OK();
            }
            else
                result.BadRequest(execute.Message);

            return Wrapper(result);
        }

        [HttpPost(template: "execute_query")]
        public async Task<IActionResult> Execute([FromBody] QueryRequest query)
        {
            StatusResponse result = new StatusResponse();
            var execute = await _context.ExecuteQuerySave(query.Query);
            if (execute.Success)
                result.OK();
            else
                result.BadRequest(execute.Message);

            return Wrapper(result);
        }
    }


}
