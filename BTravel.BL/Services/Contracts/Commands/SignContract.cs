using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.ContractRoom.Commands;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Mail;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace BTravel.BL.Services.Contracts.Commands
{
    public class SignContract : BaseService
    {
        private readonly BaseRequest _request;

        public SignContract(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractDTO> Sign(ContractDTO model)
        {
            var response = new BaseResponse<ContractDTO>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            #region Required Data Validations

            if (model == null)
            {
                response.Message = "Model data is empty";
                return response;
            }
            if (model.ContractId <= 0)
            {
                response.Message = "Invalid BookingId";
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.NameOnCreditCard))
            {
                response.Message = "Name On Credit Card is empty";
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.CardNumber))
            {
                response.Message = "Credit Card Number is empty";
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.CardExpDate))
            {
                response.Message = "Credit Card Exp Date is empty";
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.CardCVC))
            {
                response.Message = "Card CVC is empty";
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.BillingAddress))
            {
                response.Message = "Billing Address is empty";
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.PostalCode))
            {
                response.Message = "Postal Code is empty";
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.SignatureUrl))
            {
                response.Message = "Signature is empty";
                return response;
            }

            DateTime cardExp = DateTime.Now;
            var isParsed = DateTime.TryParse(model.CardExpDate, out cardExp);
            if (isParsed)
            {
                if (cardExp.ToUniversalTime() <= DateTime.UtcNow)
                {
                    response.Message = "Card Exp. date is older than today";
                    return response;
                }
            }

            if (model.RoomsList != null)
            {
                if (model.RoomsList.Count > 0)
                {
                    for (int i = 0; i < model.RoomsList.Count; i++)
                    {
                        if (string.IsNullOrWhiteSpace(model.RoomsList[i].Names))
                        {
                            response.Message = "Room(s) Names is empty, you must complete all rooms data before sign.";
                            response.ErrorType = EErrorType.InCompleteRoomsData_ForSign;
                            return response;
                        }
                    }
                }
            }

            #endregion

            var currContract = _request.Context.Contracts.FirstOrDefault(c => !c.IsDeleted && c.ContractId == model.ContractId);
            if (currContract == null)
            {
                response.Message = "Invalid BookingId";
                return response;
            }
            if (currContract.CommonUserId != _request.UserID)
            {
                response.Message = "You must be a booking owner to sign booking";
                return response;
            }
            if (currContract.StatusId == (int)EContractStatus.Sigend || currContract.IsSigned == true)
            {
                response.Message = "Booking is already signed";
                return response;
            }



            //*** Update Contract
            if (currContract.IsViewed == false)
            {
                currContract.ViewedAt = DateTime.UtcNow;
                currContract.IsViewed = true;
            }

            currContract.SignatureUrl = model.SignatureUrl;
            currContract.StatusId = (int)EContractStatus.Sigend;
            currContract.IsSigned = true;
            currContract.SignedAt = DateTime.UtcNow;

            currContract.NameOnCreditCard = AESEncryptionHelper.Encrypt(model.NameOnCreditCard);
            currContract.CardNumber = AESEncryptionHelper.Encrypt(model.CardNumber);
            currContract.CardCVC = AESEncryptionHelper.Encrypt(model.CardCVC);
            currContract.CardExpDate = AESEncryptionHelper.Encrypt(model.CardExpDate);
            currContract.BillingAddress = model.BillingAddress;
            currContract.PostalCode = model.PostalCode;

            currContract.LastModifiedAt = DateTime.UtcNow;
            currContract.LastModifiedBy = _request.UserID;

            _request.Context.SaveChanges();

            //*** Send email to customer
            var contractDTO = new GetContractById(_request).GetById(currContract.ContractId).Data;
            MailHelper.Send_ContractSigned(contractDTO);

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = $"Booking #{currContract.ContractId} has been successfully signed";

            return response;
        }
    }
}