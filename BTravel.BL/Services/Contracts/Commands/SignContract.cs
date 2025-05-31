using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.ContractRoom.Commands;
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
                response.Message = "Invalid ContractId";
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

            #endregion

            var currContract = _request.Context.Contracts.FirstOrDefault(c => !c.IsDeleted && c.ContractId == model.ContractId);
            if (currContract == null)
            {
                response.Message = "Invalid ContractId";
                return response;
            }
            if (currContract.CommonUserId != _request.UserID)
            {
                response.Message = "You must be a contract owner to sign contract";
                return response;
            }
            if (currContract.StatusId == (int)EContractStatus.Sigend || currContract.IsSigned == true)
            {
                response.Message = "Contract is already signed";
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

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = $"Contract #{currContract.ContractId} has been successfully signed";

            return response;
        }
    }
}