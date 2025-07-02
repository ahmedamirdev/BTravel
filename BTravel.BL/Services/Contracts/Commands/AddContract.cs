using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Helpers;
using BTravel.BL.Services.CommonUser.Commands;
using BTravel.BL.Services.ContractRoom.Commands;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Mail;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using BTravel.DAL.Entities;

namespace BTravel.BL.Services.Contracts.Commands
{
    public class AddContract : BaseService
    {
        private readonly BaseRequest _request;

        public AddContract(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractDTO> Add(ContractAddDTO model)
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
            if (string.IsNullOrWhiteSpace(model.HotelName) || model.NoOfRooms <= 0 || model.NoOfNights <= 0 || model.RatePerNight <= 0)
            {
                response.Message = "Hotel Name,No Of Rooms,No Of Nights or Rate Per Night is empty";
                return response;
            }

            bool isNewUser = false;
            DAL.Entities.CommonUser currCommonUser = null;
            //*** Check CommonUserId
            if (model.CommonUserId > 0)
            {
                currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => !u.IsDeleted && u.CommonUserId == model.CommonUserId);
                if (currCommonUser == null)
                {
                    response.Message = "Invalid CommonUserId";
                    return response;
                }
            }
            else
            {
                if (model.CommonUserAddDTO == null)
                {
                    response.Message = "User data is empty";
                    return response;
                }
                // Add new user
                model.CommonUserAddDTO.Password = RandomGenerator.GenerateSixNumbers();
                model.CommonUserAddDTO.RoleId = (int)ERole.Client;

                var addUserResponse = new AddCommonUser(_request).Add(model.CommonUserAddDTO);
                if (!addUserResponse.Success)
                {
                    response.Message = addUserResponse.Message;
                    return response;
                }

                currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => u.CommonUserId == addUserResponse.Data);
                isNewUser = true;
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

            //*** Add new contract data
            DAL.Entities.Contract newContract = new DAL.Entities.Contract();
            newContract.StatusId = (int)EContractStatus.Pending;
            newContract.CreatedAt = DateTime.UtcNow;
            newContract.CreatedBy = _request.UserID;
            newContract.IsDeleted = false;
            newContract.IsActive = true;
            newContract.IsViewed = false;
            newContract.IsSigned = false;
            newContract.HotelName = model.HotelName;
            newContract.NoOfRooms = model.NoOfRooms;
            newContract.CommonUserId = currCommonUser.CommonUserId;

            newContract.NoOfNights = model.NoOfNights;
            newContract.RatePerNight = model.RatePerNight;
            newContract.TaxPerc = model.TaxPerc;

            newContract.SubTotal = model.RatePerNight * model.NoOfNights;
            //newContract.Total = ((newContract.SubTotal * model.TaxPerc) / 100) + newContract.SubTotal; //*** old calculation as a percentage
            newContract.Total = newContract.SubTotal + model.TaxPerc; //*** new calculation as a decimal number

            newContract.NameOnCreditCard = AESEncryptionHelper.Encrypt(model.NameOnCreditCard);
            newContract.CardNumber = AESEncryptionHelper.Encrypt(model.CardNumber);
            newContract.CardCVC = AESEncryptionHelper.Encrypt(model.CardCVC);
            newContract.CardExpDate = AESEncryptionHelper.Encrypt(model.CardExpDate);
            newContract.BillingAddress = model.BillingAddress;
            newContract.PostalCode = model.PostalCode;

            _request.Context.Contracts.Add(newContract);
            _request.Context.SaveChanges();

            //*** Add rooms data
            if (model.Rooms != null)
            {
                if (model.Rooms.Count > 0)
                {
                    for (int i = 0; i < model.Rooms.Count; i++)
                    {
                        model.Rooms[i].ContractId = newContract.ContractId;

                        var addRoomQuery = new AddContractRoom(_request).Add(model.Rooms[i]);
                    }
                }
            }

            //*** Send email to customer
            var contractDTO = new GetContractById(_request).GetById(newContract.ContractId).Data;
            if (isNewUser)
                MailHelper.Send_NewContractNewCustomer(contractDTO, model.CommonUserAddDTO.Password);
            else
                MailHelper.Send_NewContractExistingCustomer(contractDTO);

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = $"New Booking #{newContract.ContractId} has been successfully added";

            return response;
        }
    }
}