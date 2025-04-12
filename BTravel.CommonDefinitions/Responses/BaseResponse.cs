using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.Responses
{
    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; } // where PageIndex start from 0
        public int TotalPages { get; set; }
        public int From => (PageIndex * PageSize) + 1;
        public int To => Math.Min((PageIndex + 1) * PageSize, TotalCount);
    } 

    public class BaseResponse<T>
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; } // where PageIndex start from 0
        public int TotalPages { get; set; }
        public int From => (PageIndex * PageSize) + 1; 
        public int To => Math.Min((PageIndex + 1) * PageSize, TotalCount);

        public T Data { get; set; }
    }
}