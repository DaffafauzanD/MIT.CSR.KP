using MIT.ECSR.Shared.Interface;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text;
using MIT.ECSR.Shared.Attributes;
using System;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using DocumentFormat.OpenXml.Wordprocessing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace MIT.ECSR.Shared.Helper
{
    public class GeneralHelper : IGeneralHelper
    {
        public GeneralHelper()
        {
        }

        public (bool Success, string Message, string Original, string Resize) SaveImage(string target, Guid id, FileObject file, int width, int height)
        {
            try
            {
                if (!IsFile(file.Base64))
                    return (false, "File not valid bases64", null, null);
                if (!IsImage(file.Filename))
                    return (false, "File not image format", null, null);

                string original_name = id.ToString().ToLowerInvariant() + Path.GetExtension(file.Filename).ToLowerInvariant();
                string resize_name = "sm_" + id.ToString().ToLower() + Path.GetExtension(file.Filename);
                string original_url = $"{target}/{original_name}";
                string resize_url = $"{target}/{resize_name}";

                var img = Convert.FromBase64String(file.Base64);

                //original
                File.WriteAllBytes(original_url, img);
                using (Image image = Image.Load(img))
                {
                    int new_width = image.Width;
                    int new_height = image.Height;
                    int size = width > height ? width : height;
                    int max = image.Height;
                    if (image.Width < max)
                        max = image.Width;
                    double pembagi = (double)max / (double)size;
                    if (pembagi >= 1)
                    {
                        new_width = (int)Math.Ceiling(image.Width / pembagi);
                        new_height = (int)Math.Ceiling(image.Height / pembagi);
                    }
                    else
                    {
                        pembagi = (double)size / (double)max;
                        new_width = (int)Math.Ceiling(image.Width * pembagi);
                        new_height = (int)Math.Ceiling(image.Height * pembagi);
                    }
                    int rect_x = (new_width / 2) - (width / 2);
                    int rect_y = (new_height / 2) - (height / 2);
                    image.Mutate(x => x
                         .Resize(new_width, new_height)
                         .Crop(new Rectangle(rect_x, rect_y, width, height)));

                    image.Save(resize_url);
                }
                return (true, "OK", original_url, resize_url);

            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, null);
            }
        }
        public bool IsImage(string filename)
        {
            List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".PNG" };
            return ImageExtensions.Contains(Path.GetExtension(filename).ToUpperInvariant());
        }
        public bool IsFile(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }

        #region PasswordEncrypt
        public string PasswordEncrypt(string text)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(text);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }
        #endregion

        #region Validate Password
        public bool ValidatePassword(string password)
        {
            if (password.Length >= 8 &&
                password.Any(char.IsUpper) &&
                (password.Any(char.IsSymbol) || password.Any(char.IsPunctuation)) &&
                password.Any(char.IsNumber))
                return true;
            else
                return false;

        }
        #endregion

        #region Token
        public ObjectResponse<TokenObject> DecodeToken(string token)
        {
            var result = new ObjectResponse<TokenObject>();
            try
            {
                result.Data = new TokenObject();
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var token_exp = jwt.Claims.FirstOrDefault(claim => claim.Type.Equals("exp")).Value;
                var ticks = long.Parse(token_exp);
                result.Data.RawToken = token;
                result.Data.RefreshToken = jwt.Claims.FirstOrDefault(x => x.Type == "token")?.Value;
                result.Data.ExpiredAt = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;
                result.Data.User = new TokenUserObject();

                result.Data.User.FullName = jwt.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value;
                result.Data.User.Id = Guid.Parse(jwt.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
                result.Data.User.Mail = jwt.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                result.Data.User.Username = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
                result.Data.User.Role = jwt.Claims.Where(x => x.Type == ClaimTypes.Role).Select(d => new RoleObject()
                {
                    Id = int.Parse(d.Value.Split('-')[0]),
                    Name = d.Value.Split('-')[1],
                }).FirstOrDefault();
                if (jwt.Claims.Any(d => d.Type == "company_id"))
                {
                    result.Data.User.Company = new CompanyObject()
                    {
                        Id = Guid.Parse(jwt.Claims.FirstOrDefault(x => x.Type == "company_id")?.Value),
                        Name = jwt.Claims.FirstOrDefault(x => x.Type == "company_name").Value
                    };
                }
                result.OK();
            }
            catch
            {
                result.BadRequest("Token is not recognize");
            }
            return result;
        }

        public string DoubleToRupiah(double value)
        {
            return value.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("id-ID"));
        }
        #endregion

    }
}
