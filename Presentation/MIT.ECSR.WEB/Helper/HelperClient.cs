using MIT.ECSR.Shared.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using MIT.ECSR.Shared.Interface;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;

namespace MIT.ECSR.Web.Helper
{
    public class HelperClient
    {
        public const string COOKIES_TOKEN = "MIT.ECSR.token";
       
        public static (bool Success, TokenObject Result) GetToken(HttpRequest request)
        {
            var token = request.Cookies[COOKIES_TOKEN];
            if (token != null)
            {
                return DecodeToken(token);
            }
            else
                return (false, null);
        }
        public static string RawToken(HttpRequest request)
        {
            return request.Cookies[COOKIES_TOKEN];
        }
        public static string NumberPrefix(decimal angka)
        {
            string result = System.String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("id-id"), "{0:N}", angka);
            result = result.Remove(result.Length - 3);
            return result;
        }
        public static bool IsImage(string filename)
        {
            List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".PNG" };
            return ImageExtensions.Contains(Path.GetExtension(filename).ToUpperInvariant());
        }
        public static (bool Success, TokenObject Result) DecodeToken(string token)
        {
            try
            {

                var data = new TokenObject();
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var token_exp = jwt.Claims.FirstOrDefault(claim => claim.Type.Equals("exp")).Value;
                var ticks = long.Parse(token_exp);
                data.RawToken = token;
                data.RefreshToken = jwt.Claims.FirstOrDefault(x => x.Type == "token")?.Value;
                data.ExpiredAt = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;
                data.User = new TokenUserObject();

                data.User.FullName = jwt.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value;
                data.User.Id = Guid.Parse(jwt.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
                data.User.Mail = jwt.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                data.User.Username = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
                data.User.Role = jwt.Claims.Where(x => x.Type == ClaimTypes.Role).Select(d => new RoleObject()
                {
                    Id = int.Parse(d.Value.Split('-')[0]),
                    Name = d.Value.Split('-')[1],
                }).FirstOrDefault();
                if (jwt.Claims.Any(d => d.Type == "company_id"))
                {
                    data.User.Company = new CompanyObject()
                    {
                        Id = Guid.Parse(jwt.Claims.FirstOrDefault(x => x.Type == "company_id")?.Value),
                        Name = jwt.Claims.FirstOrDefault(x => x.Type == "company_name").Value,
                        Nib = jwt.Claims.FirstOrDefault(x => x.Type == "company_nib").Value,
                        JenisPerseroan = jwt.Claims.FirstOrDefault(x => x.Type == "company_jenis").Value,
                        Npwp = jwt.Claims.FirstOrDefault(x => x.Type == "company_npwp").Value,
                        BidangUsaha = jwt.Claims.FirstOrDefault(x => x.Type == "company_bidang").Value,
                        Alamat = jwt.Claims.FirstOrDefault(x => x.Type == "company_alamat").Value,
                    };
                }
                return (true, data);
            }
            catch
            {
                return (false, null);
            }
        }

    }

}
