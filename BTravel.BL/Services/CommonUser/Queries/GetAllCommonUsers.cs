using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.CommonUser.Queries
{
    public class GetAllCommonUsers : BaseService
    {
        private readonly BaseRequest _request;

        public GetAllCommonUsers(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<IEnumerable<CommonUserDTO>> GetAll(PublicRequest model)
        {
            var response = new BaseResponse<IEnumerable<CommonUserDTO>>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var query = _request.Context.CommonUsers.Where(c => !c.IsDeleted)
                        .OrderByDescending(f => f.CommonUserId)
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
                        });

            response.TotalCount = query.Count();
            response.PageIndex = _request.PageIndex;
            response.PageSize = _request.PageSize == 0 ? Constants.defaultPageSize : _request.PageSize;
            response.TotalPages = (int)Math.Ceiling(response.TotalCount / (double)response.PageSize);

            var dto = ApplyPaging(query, _request.PageSize, _request.PageIndex);

            response.Data = dto.ToList();
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}