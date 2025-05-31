using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Helpers;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.Contracts.Queries
{
    public class GetContractByIdForSign : BaseService
    {
        private readonly BaseRequest _request;

        public GetContractByIdForSign(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractDTO> GetById(int Id)
        {
            var response = new BaseResponse<ContractDTO>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            //*** Validations
            var currContract = _request.Context.Contracts.FirstOrDefault(c => !c.IsDeleted && c.ContractId == Id);
            if (currContract == null)
            {
                response.Message = "No data found";
                return response;
            }
            if (currContract.CommonUserId != _request.UserID)
            {
                response.Message = "No data found";
                return response;
            }
            if (currContract.StatusId == (int)EContractStatus.Sigend || currContract.IsSigned == true)
            {
                response.Message = "Contract is already signed";
                return response;
            }


            var query = _request.Context.Contracts.Where(c => !c.IsDeleted && c.ContractId == Id)
                        .Select(c => new ContractDTO
                        {
                            ContractId = c.ContractId,
                            CreatedAt = c.CreatedAt.ConvertUtcToCairoTime(),
                            CreatedBy = c.CreatedBy,
                            LastModifiedAt = c.LastModifiedAt.ConvertUtcToCairoTime(),
                            LastModifiedBy = c.LastModifiedBy,
                            IsSigned = c.IsSigned,
                            SignedAt = c.SignedAt.HasValue ? c.SignedAt.Value.ConvertUtcToCairoTime() : null,
                            SignatureUrl = Constants.BaseUrl + c.SignatureUrl,
                            StatusId = c.StatusId,
                            IsViewed = c.IsViewed,
                            ViewedAt = c.ViewedAt.HasValue ? c.ViewedAt.Value.ConvertUtcToCairoTime() : null,

                            HotelName = c.HotelName,
                            NoOfRooms = c.NoOfRooms,
                            NoOfNights = c.NoOfNights,
                            RatePerNight = c.RatePerNight,
                            TaxPerc = c.TaxPerc,
                            SubTotal = c.SubTotal,
                            Total = c.Total,

                            CardNumber = AESEncryptionHelper.Decrypt(c.CardNumber),
                            NameOnCreditCard = AESEncryptionHelper.Decrypt(c.NameOnCreditCard),
                            CardCVC = AESEncryptionHelper.Decrypt(c.CardCVC),
                            CardExpDate = AESEncryptionHelper.Decrypt(c.CardExpDate),

                            BillingAddress = c.BillingAddress,
                            PostalCode = c.PostalCode,

                            CommonUser = new CommonDefinitions.DTOs.CommonUser.CommonUserDTO
                            {
                                CommonUserId = c.CommonUserId,
                                FullName = c.CommonUser.FullName,
                                PhoneNumber = c.CommonUser.PhoneNumber,
                                PrimaryMail = c.CommonUser.PrimaryMail,
                                IsPrimaryMailVerified = c.CommonUser.IsPrimaryMailVerified,
                                CreatedAt = c.CommonUser.CreatedAt.ConvertUtcToCairoTime(),
                                ImageUrl = Constants.BaseUrl + c.CommonUser.ImageUrl,
                                CompanyName = c.CommonUser.CompanyName,

                                DefaultSignatureUrl = Constants.BaseUrl + c.CommonUser.DefaultSignatureUrl,

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
                                        Comment = r.Comment,
                                        Deadline = r.Deadline,
                                        CreatedAt = r.CreatedAt.ConvertUtcToCairoTime(),
                                        CreatedBy = r.CreatedBy,
                                        ContractId = r.ContractId,
                                    }),

                            Files = c.Files.Where(f => !f.IsDeleted)
                                    .Select(r => new ContractFileDTO
                                    {
                                        FileId = r.ContractFileID,
                                        FileUrl = Constants.BaseUrl + r.FileUrl,
                                        FileName = r.FileName,
                                        CreatedAt = r.CreatedAt.ConvertUtcToCairoTime(),
                                    }),
                        }).FirstOrDefault();

            response.Data = query;
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}