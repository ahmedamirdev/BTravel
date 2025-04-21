using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using BTravel.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BTravel.BL.Services.CommonUser.Commands
{
    public class AddCommonUser : BaseService
    {
        private readonly BaseRequest _request;

        public AddCommonUser(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<int> Add(CommonUserAddDTO model)
        {
            var response = new BaseResponse<int>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            //*** Data Validations
            if (model == null)
            {
                response.Message = "Model data is empty";
                return response;
            }
            if (string.IsNullOrWhiteSpace(model.PrimaryMail) || string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.PhoneNumber) || string.IsNullOrWhiteSpace(model.Password))
            {
                response.Message = "Full Name, Email, Phone Number or Password is empty";
                return response;
            }
            if (model.RoleId != (int)ERole.Admin && model.RoleId != (int)ERole.Client)
            {
                response.Message = "Invalid RoleID";
                return response;
            }

            //check primary mail
            var isPrimaryEmailExist = _request.Context.CommonUsers.Any(c => c.PrimaryMail.ToLower() == model.PrimaryMail.ToLower() && !c.IsDeleted);
            if (isPrimaryEmailExist)
            {
                response.Message = "Primary Email Is Already Exist";
                return response;
            }
            //check phone number
            var isPhoneNumberExist = _request.Context.CommonUsers.Any(c => c.PhoneNumber.ToLower() == model.PhoneNumber.ToLower() && !c.IsDeleted);
            if (isPhoneNumberExist)
            {
                response.Message = "Phone Number Is Already Exist";
                return response;
            }

            PasswordHasher<DAL.Entities.CommonUser> hasher = new PasswordHasher<DAL.Entities.CommonUser>();
            var hashResult = hasher.HashPassword(new DAL.Entities.CommonUser(), model.Password);

            var newCommonUser = new DAL.Entities.CommonUser();

            newCommonUser.FullName = model.FullName;
            newCommonUser.PhoneNumber = model.PhoneNumber;
            newCommonUser.PrimaryMail = model.PrimaryMail;
            newCommonUser.IsPrimaryMailVerified = true;
            newCommonUser.CreatedAt = DateTime.UtcNow;
            newCommonUser.CreatedBy = _request.UserID;
            newCommonUser.Password = hashResult;
            newCommonUser.IsActive = true;
            newCommonUser.IsDeleted = false;
            newCommonUser.RoleId = model.RoleId;

            newCommonUser.CompanyName = model.CompanyName;
            newCommonUser.NameOnCreditCard = AESEncryptionHelper.Encrypt(model.NameOnCreditCard);
            newCommonUser.CardNumber = AESEncryptionHelper.Encrypt(model.CardNumber);
            newCommonUser.CardCVC = AESEncryptionHelper.Encrypt(model.CardCVC);
            newCommonUser.CardExpDate = AESEncryptionHelper.Encrypt(model.CardExpDate);
            newCommonUser.BillingAddress = model.BillingAddress;
            newCommonUser.PostalCode = model.PostalCode;

            _request.Context.CommonUsers.Add(newCommonUser);
            _request.Context.SaveChanges();

            response.Data = newCommonUser.CommonUserId;

            response.Message = $"New User #{newCommonUser.CommonUserId} Added Successfully";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}