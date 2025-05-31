using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Helpers;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.EntityFrameworkCore;

namespace BTravel.BL.Services.CommonUser.Queries
{
    public class GetCommonUserById : BaseService
    {
        private readonly BaseRequest _request;

        public GetCommonUserById(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<CommonUserDTO> GetById(int id)
        {
            var response = new BaseResponse<CommonUserDTO>();
            response.Success = false;
            response.StatusCode = HttpStatusCode.BadRequest;

            var query = _request.Context.CommonUsers
                        .Include(c => c.Role)
                        .Where(c => c.IsDeleted != true && c.CommonUserId == id)
                        .Select(c => new CommonUserDTO
                        {
                            CommonUserId = c.CommonUserId,
                            FullName = c.FullName,
                            PhoneNumber = c.PhoneNumber,
                            PrimaryMail = c.PrimaryMail,
                            IsPrimaryMailVerified = c.IsPrimaryMailVerified,
                            CreatedAt = c.CreatedAt.ConvertUtcToCairoTime(),
                            ImageUrl = Constants.BaseUrl + c.ImageUrl,
                            CompanyName = c.CompanyName,

                            DefaultSignatureUrl = Constants.BaseUrl + c.DefaultSignatureUrl,

                            RoleId = c.RoleId,
                            RoleName = c.Role.Name,

                            IsActive = c.IsActive,
                        }).FirstOrDefault();

            if (query == null)
            {
                response.Message = "No data found";
                return response;
            }

            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Data = query;

            return response;
        }
    }
}