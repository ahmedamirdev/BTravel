using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.AspNetCore.Identity;

namespace BTravel.BL.Services.CommonUser.Commands
{
    public class ChangePassword
    {
        private readonly BaseRequest _request;

        public ChangePassword(BaseRequest baseRequest)
        {
            _request = baseRequest;
        }

        public BaseResponse<bool> Change(ChangePasswordDTO model)
        {

            var response = new BaseResponse<bool>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            if (string.IsNullOrWhiteSpace(model.OldPassword) || string.IsNullOrWhiteSpace(model.NewPassword) || string.IsNullOrWhiteSpace(model.ConfirmNewPassword))
            {
                response.Message = "Old Password , New Password or Confirm New Password Is Empty";
                return response;
            }

            var user = _request.Context.CommonUsers.FirstOrDefault(u => u.CommonUserId == _request.UserID && u.IsDeleted == false);

            if (user == null)
            {
                response.Message = "Invalid User ID";
                return response;

            }

            PasswordHasher<DAL.Entities.CommonUser> hasher = new PasswordHasher<DAL.Entities.CommonUser>();
            var compareHash = hasher.VerifyHashedPassword(user, user.Password, model.OldPassword);
            if (compareHash == PasswordVerificationResult.Failed)
            {
                response.Message = "Invalid Old Password";
                return response;
            }

            if (model.NewPassword != model.ConfirmNewPassword)
            {
                response.Message = "New Password does not match Confirm New Password";
                return response;

            }

            user.Password = hasher.HashPassword(user, model.NewPassword);
            _request.Context.SaveChanges();

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            response.Message = "Password Changed Successfully";


            return response;

        }

    }
}
