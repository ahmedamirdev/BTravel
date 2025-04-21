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
    public class GetContractById : BaseService
    {
        private readonly BaseRequest _request;

        public GetContractById(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractDTO> GetById(int Id)
        {
            var response = new BaseResponse<ContractDTO>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var query = _request.Context.Contracts.Where(c => !c.IsDeleted && c.ContractId == Id)
                        .Select(c => new ContractDTO
                        {
                            ContractId = c.ContractId,
                            CreatedAt = c.CreatedAt.AddHours(2),
                            CreatedBy = c.CreatedBy,
                            LastModifiedAt = c.LastModifiedAt.AddHours(2),
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
                                CreatedAt = c.CommonUser.CreatedAt.AddHours(2),
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
                                        CreatedAt = r.CreatedAt.AddHours(2),
                                        CreatedBy = r.CreatedBy,
                                        ContractId = r.ContractId,
                                    }),

                            Files = c.Files.Where(f => !f.IsDeleted)
                                    .Select(r => new ContractFileDTO
                                    {
                                        FileId = r.ContractFileID,
                                        FileUrl = Constants.BaseUrl + r.FileUrl,
                                        FileName = r.FileName,
                                        CreatedAt = r.CreatedAt.AddHours(2),
                                    }),
                        }).FirstOrDefault();

            if (query == null)
            {
                response.Message = "No data found";
                return response;
            }

            response.Data = query;
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}