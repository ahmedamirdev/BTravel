using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.CommonUser.Queries
{
    public class GetCommonUserById : BaseService
    {
        private readonly BaseRequest _baseRequest;

        public GetCommonUserById(BaseRequest baseRequest)
        {
            _baseRequest = baseRequest;
            
        }

        public BaseResponse<CommonUserDTO> GetById(int id) 
        {
            var Response = new BaseResponse<CommonUserDTO>();
            Response.Success = false;
            Response.StatusCode = HttpStatusCode.BadRequest;

            var query = _baseRequest.Context.CommonUsers.Where(c => c.IsDeleted != true && c.CommonUserId == id)
                .Select(c => new CommonUserDTO
                {
                    id =c.CommonUserId,
                    Name = c.FullName

                }).FirstOrDefault();

            Response.Success = true;
            Response.StatusCode = HttpStatusCode.OK;
            Response.Data = query;

            return Response;

        }
    }
}
