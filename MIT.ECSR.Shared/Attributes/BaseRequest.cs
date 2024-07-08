using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ECSR.Shared.Attributes
{
    public class QueryRequest
    {
        public string Query { get; set; }
    }
    public class ListRequest
    {
        public List<FilterRequest> Filter { get; set; }

        [Required]
        public SortRequest Sort { get; set; } = null!;
        public int? Start { get; set; }
        public int? Length { get; set; }
    }
    public class FilterRequest
    {
        public string Field { get; set; } = null!;
        public string Search { get; set; } = null!;
    }

    public class SortRequest
    {
        public string Field { get; set; } = null!;
        public SortTypeEnum Type { get; set; }
    }
    public enum SortTypeEnum
    {
        ASC,
        DESC
    }
    public class AddRequest<T>
    {
        public T Data { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime CreateDate { get { return DateTime.Now; } }
    }
    public class UpdateRequest<T>
    {
        public T Data { get; set; } = default!;
        public string UpdateBy { get; set; } = null!;
        public DateTime UpdateDate { get { return DateTime.Now; } }
    }
}
