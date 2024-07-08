using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Shared.Attributes
{
    public class TokenObject
    {
        public TokenUserObject User { get; set; }
        public DateTime ExpiredAt { get; set; }
        public string RawToken { get; set; }
        public string RefreshToken { get; set; }
    }
    public class TokenUserObject
    {
        public Guid Id { get; set; }
        public RoleObject Role { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public CompanyObject Company { get; set; }
        public MediaUrl Photos { get; set; }
    }
    public class CompanyObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Nib { get; set; }
        public string JenisPerseroan { get; set; }
        public string Npwp { get; set; }
        public string BidangUsaha { get; set; }
        public string Alamat { get; set; }
    }
    public class RoleObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public enum RoleName
    {
        DPMPTSPP,
        BAPPEDA,
        FORUM,
        ECBANG,
        OPD,
        PERANGKAT
    }

    public class MediaUrl
    {
        public Guid Id { get; set; }
        public string Original { get; set; }
        public string Resize { get; set; }
        public string Filename { get; set; }
    }
}
