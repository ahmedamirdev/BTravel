using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.Security.Encryption;
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
            var Response = new BaseResponse<CommonUserDTO>();
            Response.Success = false;
            Response.StatusCode = HttpStatusCode.BadRequest;

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
                            CreatedAt = c.CreatedAt,
                            ImageUrl = c.ImageUrl,
                            CompanyName = c.CompanyName,
                            CardNumber = AESEncryptionHelper.Decrypt(c.CardNumber),
                            CardExpDate = AESEncryptionHelper.Decrypt(c.CardExpDate),
                            CardCVC = AESEncryptionHelper.Decrypt(c.CardCVC),
                            NameOnCreditCard = AESEncryptionHelper.Decrypt(c.NameOnCreditCard),

                            BillingAddress = c.BillingAddress,
                            PostalCode = c.PostalCode,
                            DefaultSignatureUrl = c.DefaultSignatureUrl,

                            RoleId = c.RoleId,
                            RoleName = c.Role.Name,

                        }).FirstOrDefault();

            Response.Success = true;
            Response.StatusCode = HttpStatusCode.OK;
            Response.Data = query;

            return Response;

        }
    }
}