using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MIT.ECSR.Shared.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;
using MIT.ECSR.Web.Helper;
using MIT.ECSR.Web.Model;
using MIT.ECSR.Core.Response;

namespace MIT.ECSR.Web.Helper
{
    public interface IRestAPIHelper
    {
        Task<ObjectResponse<object>> DetailProgram(Guid id);
        Task<ObjectResponse<object>> DetailUser(Guid id);
        Task<ObjectResponse<ProgramItemResponse>> DetailProgramItem(Guid id);
        Task<ListResponse<object>> ItemProgram(Guid id_program, bool approve_only, bool vendor_only);
        Task<ListResponse<ProgressProgramResponse>> ProgressProgram(Guid id_program, bool show_all_status);
        Task<ObjectResponse<object>> DetailPenawaran(Guid id, string token);
        Task<ListResponse<object>> ListJenisProgram();
        Task<ListResponse<object>> ListDati();
    }
    public class RestAPIHelper : IRestAPIHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<RestAPIHelper> _logger;
        private string BASE_URL = "";
        private string VERSIONING = "v1";
        public RestAPIHelper(IHttpContextAccessor httpContextAccessor, ILogger<RestAPIHelper> logger, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            BASE_URL = configuration.GetValue<string>("APIBaseUrl");
        }

        public async Task<ObjectResponse<object>> DetailProgram(Guid id)
        {
            string url = $"{BASE_URL}/{VERSIONING}/program/get/{id}";
            return await DoRequest<ObjectResponse<object>>(url, Method.GET, null, true);
        }
        public async Task<ObjectResponse<object>> DetailUser(Guid id)
        {
            string url = $"{BASE_URL}/{VERSIONING}/user/get/{id}";
            return await DoRequest<ObjectResponse<object>>(url, Method.GET, null, false);
        }
        public async Task<ObjectResponse<ProgramItemResponse>> DetailProgramItem(Guid id)
        {
            string url = $"{BASE_URL}/{VERSIONING}/ProgramItem/get/{id}";
            return await DoRequest<ObjectResponse<ProgramItemResponse>>(url, Method.GET, null, false);
        }
        public async Task<ListResponse<object>> ItemProgram(Guid id_program,bool approve_only,bool vendor_only)
        {
            string url = $"{BASE_URL}/{VERSIONING}/ProgramItem/list_all/{id_program}/{approve_only}/{vendor_only}";
            return await DoRequest<ListResponse<object>>(url, Method.GET, null, true);
        }
        public async Task<ListResponse<ProgressProgramResponse>> ProgressProgram(Guid id_program,bool show_all_status)
        {
            var param = new ListRequest()
            {
                Sort = new SortRequest()
                {
                    Field = "tglprogress",
                    Type = SortTypeEnum.ASC
                },
                Filter = new List<FilterRequest>()
                {
                    new FilterRequest()
                    {
                        Field = "idprogram",
                        Search = id_program.ToString(),
                    }
                }
            };
            if (!show_all_status)
                param.Filter.Add(new FilterRequest()
                {
                    Field = "status",
                    Search = "2"
                });
            string url = $"{BASE_URL}/{VERSIONING}/ProgressProgram/list";
            return await DoRequest<ListResponse<ProgressProgramResponse>>(url, Method.POST, param, true);
        }
        public async Task<ObjectResponse<object>> DetailPenawaran(Guid id, string token)
        {
            string url = $"{BASE_URL}/{VERSIONING}/penawaran/get/{id}";
            return await DoRequest<ObjectResponse<object>>(url, Method.GET, new { token = token }, false);
        }
        public async Task<ListResponse<object>> ListJenisProgram()
        {
            var body = new ListRequest()
            {
                Sort = new SortRequest()
                {
                    Field = "id",
                    Type = SortTypeEnum.ASC
                }
            };
            string url = $"{BASE_URL}/{VERSIONING}/jenisProgram/list";
            return await DoRequest<ListResponse<object>>(url, Method.POST, body, true);
        }

        public async Task<ListResponse<object>> ListDati()
        {
            var body = new ListRequest()
            {
                Sort = new SortRequest()
                {
                    Field = "id",
                    Type = SortTypeEnum.ASC
                }
            };
            string url = $"{BASE_URL}/{VERSIONING}/dati/list";
            return await DoRequest<ListResponse<object>>(url, Method.POST, body, true);
        }

        #region Do Request Utility
        private async Task<T> DoRequest<T>(string url, Method method, object body, bool isAnnonymous) where T : class
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(method);
                request.AddHeader("Content-Type", "application/json");
                client.Timeout = -1;
                if (body != null)
                {
                    var reqBody = JsonConvert.SerializeObject(body);
                    request.AddParameter("application/json",
                                reqBody,
                                ParameterType.RequestBody);
                }
                if (!isAnnonymous)
                {
                    string token = _httpContextAccessor.HttpContext.Request.Cookies[HelperClient.COOKIES_TOKEN];
                    request.AddHeader("Authorization", $"Bearer {token}");
                }
                IRestResponse response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}