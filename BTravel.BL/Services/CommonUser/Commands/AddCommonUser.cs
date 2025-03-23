using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using BTravel.DAL.Entities;

namespace BTravel.BL.Services.CommonUser.Commands
{
    public class AddCommonUser : BaseService
    {
        private readonly BaseRequest _request;

        public AddCommonUser(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<int> Add(AddCommonUserDTO model)
        {
            var response = new BaseResponse<int>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var isPrimaryEmailExist = _request.Context.CommonUsers.Any(c => c.PrimaryMail.ToLower() == model.PrimaryMail.ToLower() && !c.IsDeleted);
            if (isPrimaryEmailExist)
            {
                response.Message = "Primary Email Is Already Exist ";
                return response;
            }


            var isPhoneNumberExist = _request.Context.CommonUsers.Any(c => c.PhoneNumber.ToLower() == model.PhoneNumber.ToLower() && !c.IsDeleted);
            if (isPrimaryEmailExist)
            {
                response.Message = "Phone Number Is Already Exist ";
                return response;
            }

            var isModelValid = model.RoleId == (int)ERole.Admin || model.RoleId == (int)ERole.Client;
            if (!isModelValid)
            {
                response.Message = "Invalid Role ID";
                return response;
            }

            var newCommonUser = new DAL.Entities.CommonUser();

            newCommonUser.FullName = model.FullName;
            newCommonUser.PhoneNumber = model.PhoneNumber;
            newCommonUser.PrimaryMail = model.PrimaryMail;
            newCommonUser.IsPrimaryMailVerified = true;
            newCommonUser.CreatedAt = DateTime.UtcNow;
            newCommonUser.CreatedBy = _request.UserID;
            newCommonUser.Password = model.Password;
            newCommonUser.IsActive = true;
            newCommonUser.IsDeleted = false;
            newCommonUser.RoleId = model.RoleId;

            _request.Context.CommonUsers.Add(newCommonUser);
            _request.Context.SaveChanges();

            response.Data = newCommonUser.CommonUserId;

            response.Message = "New Common User Added Successfully ";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }


    }
}
