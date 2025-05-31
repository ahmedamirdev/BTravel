using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.CommonUser.Commands;
using BTravel.BL.Services.ContractRoom.Commands;
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
                model.CommonUserAddDTO.Password = "123";
                model.CommonUserAddDTO.RoleId = (int)ERole.Client;

                var addUserResponse = new AddCommonUser(_request).Add(model.CommonUserAddDTO);
                if (!addUserResponse.Success)
                {
                    response.Message = addUserResponse.Message;
                    return response;
                }

                currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => u.CommonUserId == addUserResponse.Data);
            }

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
            newContract.Total = ((newContract.SubTotal * model.TaxPerc) / 100) + newContract.SubTotal;

            newContract.NameOnCreditCard = AESEncryptionHelper.Encrypt(model.NameOnCreditCard);
            newContract.CardNumber = AESEncryptionHelper.Encrypt(model.CardNumber);
            newContract.CardCVC = AESEncryptionHelper.Encrypt(model.CardCVC);
            newContract.CardExpDate = AESEncryptionHelper.Encrypt(model.CardExpDate);
            newContract.BillingAddress = model.BillingAddress;
            newContract.PostalCode = model.PostalCode;

            _request.Context.Contracts.Add(newContract);
            _request.Context.SaveChanges();

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

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = $"New Contract #{newContract.ContractId} has been successfully added";

            return response;
        }
    }
}