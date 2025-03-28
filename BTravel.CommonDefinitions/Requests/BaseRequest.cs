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

        public string BaseUrl { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }

        public bool IsDesc { get; set; }
        public string OrderByColumn { get; set; }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public int UserID { get; set; }
        public int RoleID { get; set; }

        public string Search { get; set; }
    }
}