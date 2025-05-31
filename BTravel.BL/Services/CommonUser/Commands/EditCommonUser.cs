using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.AspNetCore.Identity;

namespace BTravel.BL.Services.CommonUser.Commands
{
    public class EditCommonUser : BaseService
    {
        private readonly BaseRequest _request;

        public EditCommonUser(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<int> Edit(CommonUserAddDTO model)
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
            if (string.IsNullOrWhiteSpace(model.PrimaryMail) || string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                response.Message = "Full Name, Email or Phone Number is empty";
                return response;
            }
            if (model.RoleId != (int)ERole.Admin && model.RoleId != (int)ERole.Client)
            {
                response.Message = "Invalid RoleID";
                return response;
            }

            //check primary mail
            var isPrimaryEmailExist = _request.Context.CommonUsers.Any(c => c.PrimaryMail.ToLower() == model.PrimaryMail.ToLower() && !c.IsDeleted && c.CommonUserId != model.CommonUserId);
            if (isPrimaryEmailExist)
            {
                response.Message = "Primary Email Is Already Exist";
                return response;
            }
            //check phone number
            var isPhoneNumberExist = _request.Context.CommonUsers.Any(c => c.PhoneNumber.ToLower() == model.PhoneNumber.ToLower() && !c.IsDeleted && c.CommonUserId != model.CommonUserId);
            if (isPhoneNumberExist)
            {
                response.Message = "Phone Number Is Already Exist";
                return response;
            }

            var currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => u.CommonUserId == model.CommonUserId && !u.IsDeleted);
            if (currCommonUser == null)
            {
                response.Message = "Invalid CommonUserId";
                return response;
            }

            currCommonUser.FullName = model.FullName;
            currCommonUser.PhoneNumber = model.PhoneNumber;
            currCommonUser.PrimaryMail = model.PrimaryMail;
            currCommonUser.IsPrimaryMailVerified = true;
            currCommonUser.RoleId = model.RoleId;
            currCommonUser.CompanyName = model.CompanyName;

            currCommonUser.LastModifiedAt = DateTime.UtcNow;
            currCommonUser.LastModifiedBy = _request.UserID;

            //*** Update password if not empty
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                PasswordHasher<DAL.Entities.CommonUser> hasher = new PasswordHasher<DAL.Entities.CommonUser>();
                var hashResult = hasher.HashPassword(new DAL.Entities.CommonUser(), model.Password);

                currCommonUser.Password = hashResult;
            }
            
            _request.Context.SaveChanges();

            response.Message = $"User #{currCommonUser.CommonUserId} Updated Successfully";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}