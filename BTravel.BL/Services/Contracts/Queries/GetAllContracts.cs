using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.Contracts.Queries
{
    public class GetAllContracts : BaseService
    {
        private readonly BaseRequest _request;

        public GetAllContracts(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<IEnumerable<ContractDTO>> GetAll(PublicRequest model)
        {
            var response = new BaseResponse<IEnumerable<ContractDTO>>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var query = _request.Context.Contracts.Where(c => !c.IsDeleted)
                        .OrderByDescending(f => f.ContractId)
                        .Select(c => new ContractDTO
                        {
                            ContractId = c.ContractId,
                            CreatedAt = c.CreatedAt,
                            CreatedBy = c.CreatedBy,
                            LastModifiedAt = c.LastModifiedAt,
                            LastModifiedBy = c.LastModifiedBy,
                            SignedAt = c.SignedAt,
                            SignatureUrl = c.SignatureUrl,
                            StatusId = c.StatusId,

                            HotelName = c.HotelName,
                            NoOfRooms = c.NoOfRooms,
                            NoOfNights = c.NoOfNights,
                            RatePerNight = c.RatePerNight,
                            TaxPerc = c.TaxPerc,
                            SubTotal = c.SubTotal,
                            Total = c.Total,

                            CommonUser = new CommonDefinitions.DTOs.CommonUser.CommonUserDTO
                            {
                                CommonUserId = c.CommonUserId,
                                FullName = c.CommonUser.FullName,
                                PhoneNumber = c.CommonUser.PhoneNumber,
                                PrimaryMail = c.CommonUser.PrimaryMail,
                                IsPrimaryMailVerified = c.CommonUser.IsPrimaryMailVerified,
                                CreatedAt = c.CommonUser.CreatedAt,
                                ImageUrl = c.CommonUser.ImageUrl,
                                CompanyName = c.CommonUser.CompanyName,

                                CardNumber = AESEncryptionHelper.Decrypt(c.CommonUser.CardNumber),
                                NameOnCreditCard = AESEncryptionHelper.Decrypt(c.CommonUser.NameOnCreditCard),
                                CardCVC = AESEncryptionHelper.Decrypt(c.CommonUser.CardCVC),
                                CardExpDate = AESEncryptionHelper.Decrypt(c.CommonUser.CardExpDate),

                                BillingAddress = c.CommonUser.BillingAddress,
                                PostalCode = c.CommonUser.PostalCode,
                                DefaultSignatureUrl = c.CommonUser.DefaultSignatureUrl,

                                RoleId = c.CommonUser.RoleId,
                                RoleName = c.CommonUser.Role.Name,
                            },

                            Rooms = c.Rooms.Where(r => !r.IsDeleted)
                                    .Select(r => new ContractRoomDTO
                                    {
                                        ContractRoomId = r.ContractRoomId,
                                        RoomType = r.RoomType,
                                        Names = r.Names,
                                        CheckIn = r.CheckIn,
                                        CheckOut = r.CheckOut,
                                        NumOfNights = r.NumOfNights,
                                        RoomAmenities = r.RoomAmenities,
                                        CreatedAt = r.CreatedAt,
                                        CreatedBy = r.CreatedBy,
                                        ContractId = r.ContractId,
                                    }),
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