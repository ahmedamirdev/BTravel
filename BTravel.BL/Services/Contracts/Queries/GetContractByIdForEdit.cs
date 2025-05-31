using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.Contracts.Queries
{
    public class GetContractByIdForEdit : BaseService
    {
        private readonly BaseRequest _request;

        public GetContractByIdForEdit(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractEditDTO> GetById(int Id)
        {
            var response = new BaseResponse<ContractEditDTO>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var query = _request.Context.Contracts.Where(c => !c.IsDeleted && c.ContractId == Id)
                        .Select(c => new ContractEditDTO
                        {
                            ContractId = c.ContractId,

                            HotelName = c.HotelName,
                            NoOfRooms = c.NoOfRooms,
                            NoOfNights = c.NoOfNights,
                            RatePerNight = c.RatePerNight,
                            TaxPerc = c.TaxPerc,

                            CardNumber = AESEncryptionHelper.Decrypt(c.CardNumber),
                            NameOnCreditCard = AESEncryptionHelper.Decrypt(c.NameOnCreditCard),
                            CardCVC = AESEncryptionHelper.Decrypt(c.CardCVC),
                            CardExpDate = AESEncryptionHelper.Decrypt(c.CardExpDate),

                            BillingAddress = c.BillingAddress,
                            PostalCode = c.PostalCode,

                            Rooms = c.Rooms.Where(r => !r.IsDeleted)
                                    .Select(r => new ContractRoomAddDTO
                                    {
                                        RoomType = r.RoomType,
                                        Names = r.Names,
                                        CheckIn = r.CheckIn,
                                        CheckOut = r.CheckOut,
                                        NumOfNights = r.NumOfNights,
                                        RoomAmenities = r.RoomAmenities,
                                        Comment = r.Comment,
                                        Deadline = r.Deadline,
                                        ContractId = r.ContractId,
                                    }).ToList(),
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