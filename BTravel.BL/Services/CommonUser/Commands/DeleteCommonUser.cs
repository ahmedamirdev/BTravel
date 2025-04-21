using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.CommonUser.Commands
{
    public class DeleteCommonUser : BaseService
    {
        private readonly BaseRequest _request;

        public DeleteCommonUser(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<bool> Delete(int Id)
        {
            var response = new BaseResponse<bool>();
            response.Success = false;
            response.Data = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => u.CommonUserId == Id && !u.IsDeleted);
            if (currCommonUser == null)
            {
                response.Message = "Invalid CommonUserId";
                return response;
            }

            if (_request.RoleID != (int)ERole.Admin)
            {
                response.Message = "Admin only can delete the user";
                return response;
            }

            currCommonUser.IsDeleted = true;
            currCommonUser.LastModifiedAt = DateTime.UtcNow;
            currCommonUser.LastModifiedBy = _request.UserID;
            currCommonUser.DeletedAt = DateTime.UtcNow;
            currCommonUser.DeletedBy = _request.UserID;

            _request.Context.SaveChanges();

            response.Success = true;
            response.Data = true;
            response.Message = $"User #{currCommonUser.CommonUserId} deleted successfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}