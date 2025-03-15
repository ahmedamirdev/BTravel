using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BTravel.DAL;

namespace BTravel.CommonDefinitions.Requests
{
    public class BaseRequest
    {
        public BTravelDbContext Context { get; set; }

        public bool IsDesc { get; set; }

        public string OrderByColumn { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string BaseUrl { get; set; }

        public long LanguageId { get; set; }
        public string Language { get; set; }

        public long UserID { get; set; }

        public string Search { get; set; }
    }
}
