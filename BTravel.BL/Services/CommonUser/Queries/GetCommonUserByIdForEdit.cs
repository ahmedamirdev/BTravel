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
    public class GetCommonUserByIdForEdit : BaseService
    {
        private readonly BaseRequest _request;

        public GetCommonUserByIdForEdit(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<CommonUserAddDTO> GetById(int id)
        {
            var response = new BaseResponse<CommonUserAddDTO>();
            response.Success = false;
            response.StatusCode = HttpStatusCode.BadRequest;

            var query = _request.Context.CommonUsers
                        .Where(c => c.IsDeleted != true && c.CommonUserId == id)
                        .Select(c => new CommonUserAddDTO
                        {
                            CommonUserId = c.CommonUserId,
                            FullName = c.FullName,
                            PhoneNumber = c.PhoneNumber,
                            PrimaryMail = c.PrimaryMail,
                            CompanyName = c.CompanyName,

                            CardNumber = AESEncryptionHelper.Decrypt(c.CardNumber),
                            CardExpDate = AESEncryptionHelper.Decrypt(c.CardExpDate),
                            CardCVC = AESEncryptionHelper.Decrypt(c.CardCVC),
                            NameOnCreditCard = AESEncryptionHelper.Decrypt(c.NameOnCreditCard),

                            BillingAddress = c.BillingAddress,
                            PostalCode = c.PostalCode,

                            RoleId = c.RoleId,
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