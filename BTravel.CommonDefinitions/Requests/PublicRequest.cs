using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.Requests
{
    public class PublicRequest
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Search { get; set; }
    }

    public class PublicRequest<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Search { get; set; }

        public T Filter { get; set; }
    }
}