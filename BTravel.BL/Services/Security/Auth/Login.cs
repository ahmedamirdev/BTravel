using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.Auth;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.AspNetCore.Identity;

namespace BTravel.BL.Services.Security.Auth
{
    public class Login : BaseService
    {
        private readonly BaseRequest _request;

        public Login(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<CommonUserDTO> CheckCredentials(LoginDTO model)
        {
            var response = new BaseResponse<CommonUserDTO>();
            response.Data = null;
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            ////*** Check data is not empty
            //if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            //{
            //    response.Message = "Email or Password is empty";
            //    return response;
            //}
            ////*** Check Email
            //var currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => u.PrimaryEmail == model.Email && !u.IsDeleted.Value);
            //if (currCommonUser == null)
            //{
            //    response.Message = "Invalid Email or Password";
            //    return response;
            //}
            //else
            //{
            //    //*** Check Password
            //    PasswordHasher<DAL.DB.CommonUser> hasher = new PasswordHasher<DAL.DB.CommonUser>();
            //    var compareHash = hasher.VerifyHashedPassword(currCommonUser, currCommonUser.HashedPassword, model.Password);
            //    if (compareHash == PasswordVerificationResult.Failed)
            //    {
            //        response.Message = "Invalid Email or Password";
            //        return response;
            //    }

            //    if (currCommonUser.IsActive.Value)
            //    {
            //        //*** UnSuspend User if suspended
            //        if (currCommonUser.IsSuspended.Value)
            //        {
            //            var request = new BaseRequest
            //            {
            //                Context = _request.Context,
            //            };

            //            UnSuspendUser query = new UnSuspendUser(request);
            //            var UnSuspendUserResponse = query.UnSuspend(currCommonUser.CommonUserId);
            //        }

            //        //*** Update LastLoginAt
            //        currCommonUser.LastLoginAt = DateTime.UtcNow;

            //        //*** Update FCM Token
            //        currCommonUser.FcmToken = model.FcmToken;

            //        currCommonUser.LastModifiedAt = DateTime.UtcNow;

            //        _request.Context.SaveChanges();

            //        response.Success = true;
            //        response.Message = "Login Succeeded";
            //        response.StatusCode = HttpStatusCode.OK;
            //        response.Data = new LoginResponseDTO
            //        {
            //            Token = new TokenManager(_request).CreateJwtToken(currCommonUser),

            //            User = new GetUserById(_request).GetById(currCommonUser.CommonUserId).Data,
            //        };
            //    }
            //    else //*** IsActive = False
            //    {
            //        if (currCommonUser.LastLoginAt.HasValue) //*** Not First Login
            //        {
            //            response.Message = "User is not Active. Please contact support via support@finds.com";
            //            return response;
            //        }
            //        else //*** First Login and user didn't activate his account
            //        {
            //            response.Message = "Login Failed. Please verify your email";
            //            return response;
            //        }
            //    }
            //}

            return response;
        }
    }
}