//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MIT.ECSR.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MIT.ECSR.Data.Model;
using MIT.ECSR.Shared.Attributes;

namespace MIT.ECSR.Core.Response
{
    public partial class UserResponse : IMapResponse<UserResponse, MIT.ECSR.Data.Model.SetUser>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public ReferensiObject Role { get; set; }
        public PerusahaanResponse Perusahaan { get; set; }
        public MediaUrl Photo { get; set; }

        public void Mapping(IMappingExpression<MIT.ECSR.Data.Model.SetUser, UserResponse> map)
        {
            //use this for mapping
            map.ForMember(d => d.Status, opt => opt.MapFrom(s => CheckStatus(s)))
                .ForMember(d => d.Role, opt => opt.MapFrom(s => new ReferensiObject()
                {
                    Id = s.IdRole,
                    Nama = s.IdRoleNavigation.Name
                }))
                .ForMember(d => d.Perusahaan, opt => opt.MapFrom(s => s.MstPerusahaan != null ? new PerusahaanResponse()
                {
                    Alamat = s.MstPerusahaan.FirstOrDefault().Alamat,
                    BidangUsaha = s.MstPerusahaan.FirstOrDefault().BidangUsaha,
                    Id = s.MstPerusahaan.FirstOrDefault().Id,
                    NamaPerusahaan = s.MstPerusahaan.FirstOrDefault().NamaPerusahaan,
                } : null));
        }
        private string CheckStatus(MIT.ECSR.Data.Model.SetUser s)
        {
            if (!s.Active)
                return "Not Active";
            else
                return "Active";
        }
    }
}

