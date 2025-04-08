using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.CommonUser.Queries;
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

            //*** Check data is not empty
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                response.Message = "Email or Password is empty";
                return response;
            }
            //*** Check Email
            var currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => u.PrimaryMail == model.Email && !u.IsDeleted);
            if (currCommonUser == null)
            {
                response.Message = "Invalid Email or Password";
                return response;
            }
            else
            {
                //*** Check Password
                PasswordHasher<DAL.Entities.CommonUser> hasher = new PasswordHasher<DAL.Entities.CommonUser>();
                var compareHash = hasher.VerifyHashedPassword(currCommonUser, currCommonUser.Password, model.Password);
                if (compareHash == PasswordVerificationResult.Failed)
                {
                    response.Message = "Invalid Email or Password";
                    return response;
                }

                if (currCommonUser.IsActive)
                {
                    //*** Update LastLoginAt
                    currCommonUser.LastLoginAt = DateTime.UtcNow;

                    _request.Context.SaveChanges();

                    response.Success = true;
                    response.Message = "Login Succeeded";
                    response.StatusCode = HttpStatusCode.OK;
                    response.Data = new GetCommonUserById(_request).GetById(currCommonUser.CommonUserId).Data;
                }
                else //*** IsActive = False
                {
                    response.Message = "User is not Active. Please contact support";
                    return response;
                }
            }

            return response;
        }
    }
}