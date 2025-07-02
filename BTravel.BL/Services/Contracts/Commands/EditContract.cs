using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.CommonUser.Commands;
using BTravel.BL.Services.ContractRoom.Commands;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Mail;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.Contracts.Commands
{
    public class EditContract : BaseService
    {
        private readonly BaseRequest _request;

        public EditContract(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractDTO> Edit(ContractEditDTO model)
        {
            var response = new BaseResponse<ContractDTO>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            //*** Validations
            if (model == null)
            {
                response.Message = "Model data is empty";
                return response;
            }
            if (model.ContractId <= 0 || string.IsNullOrWhiteSpace(model.HotelName) || model.NoOfRooms <= 0 || model.NoOfNights <= 0 || model.RatePerNight <= 0)
            {
                response.Message = "BookingId, Hotel Name,No Of Rooms,No Of Nights or Rate Per Night is empty";
                return response;
            }
            if (_request.RoleID != (int)ERole.Admin)
            {
                response.Message = "You must be Admin to edit this booking";
                return response;
            }

            var currContract = _request.Context.Contracts.FirstOrDefault(c => !c.IsDeleted && c.ContractId == model.ContractId);
            if (currContract == null)
            {
                response.Message = "Invalid BookingId";
                return response;
            }

            if (model.Rooms != null)
            {
                if (model.Rooms.Count > 0)
                {
                    for (int i = 0; i < model.Rooms.Count; i++)
                    {
                        if (model.Rooms[i].CheckIn.Day < DateTime.UtcNow.Day || model.Rooms[i].CheckOut.Day < DateTime.UtcNow.Day)
                        {
                            response.Message = "CheckIn or CheckOut date is older than today";
                            return response;
                        }
                        if (model.Rooms[i].CheckIn.Day > model.Rooms[i].CheckOut.Day)
                        {
                            response.Message = "CheckIn date is older than CheckOut date";
                            return response;
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(model.CardExpDate))
            {
                DateTime cardExp = DateTime.Now;
                var isParsed = DateTime.TryParse(model.CardExpDate, out cardExp);
                if (isParsed)
                {
                    if (cardExp.Day <= DateTime.UtcNow.Day)
                    {
                        response.Message = "Card Exp. date is older than today";
                        return response;
                    }
                }
            }

            //if (currContract.StatusId == (int)EContractStatus.Sigend)
            //{
            //    response.Message = "Contract has been signed and can not be updated";
            //    return response;
            //}

            // currContract.StatusId = (int)EContractStatus.Pending; //*** Ask Mahmoud ??
            currContract.HotelName = model.HotelName;
            currContract.NoOfRooms = model.NoOfRooms;
            currContract.NoOfNights = model.NoOfNights;
            currContract.RatePerNight = model.RatePerNight;
            currContract.TaxPerc = model.TaxPerc;

            currContract.SubTotal = model.RatePerNight * model.NoOfNights;
            //currContract.Total = ((currContract.SubTotal * model.TaxPerc) / 100) + currContract.SubTotal; //*** old calculation as a percentage
            currContract.Total = currContract.SubTotal + model.TaxPerc; //*** new calculation as a decimal number

            currContract.NameOnCreditCard = AESEncryptionHelper.Encrypt(model.NameOnCreditCard);
            currContract.CardNumber = AESEncryptionHelper.Encrypt(model.CardNumber);
            currContract.CardCVC = AESEncryptionHelper.Encrypt(model.CardCVC);
            currContract.CardExpDate = AESEncryptionHelper.Encrypt(model.CardExpDate);
            currContract.BillingAddress = model.BillingAddress;
            currContract.PostalCode = model.PostalCode;

            currContract.LastModifiedAt = DateTime.UtcNow;
            currContract.LastModifiedBy = _request.UserID;

            _request.Context.SaveChanges();

            //*** Delete old rooms
            var currRooms = _request.Context.ContractRooms.Where(u => u.ContractId == currContract.ContractId && !u.IsDeleted).ToList();
            if (currRooms != null)
            {
                for (int i = 0; i < currRooms.Count; i++)
                {
                    currRooms[i].IsDeleted = true;
                }
                _request.Context.SaveChanges();
            }

            //*** Add new rooms
            if (model.Rooms != null)
            {
                if (model.Rooms.Count > 0)
                {
                    for (int i = 0; i < model.Rooms.Count; i++)
                    {
                        model.Rooms[i].ContractId = currContract.ContractId;

                        var addRoomQuery = new AddContractRoom(_request).Add(model.Rooms[i]);
                    }
                }
            }

            //*** Send email to customer
            var contractDTO = new GetContractById(_request).GetById(currContract.ContractId).Data;
            MailHelper.Send_ContractUpdated(contractDTO);

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = $"Booking #{currContract.ContractId} has been successfully updated";

            return response;
        }
    }
}