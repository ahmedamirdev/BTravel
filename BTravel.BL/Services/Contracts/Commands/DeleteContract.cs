using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.Contracts.Commands
{
    public class DeleteContract : BaseService
    {
        private readonly BaseRequest _request;

        public DeleteContract(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<bool> Delete(int Id)
        {
            var response = new BaseResponse<bool>();
            response.Success = false;
            response.Data = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var currContract = _request.Context.Contracts.FirstOrDefault(u => u.ContractId == Id && !u.IsDeleted);
            if (currContract == null)
            {
                response.Message = "Invalid BookingId";
                return response;
            }

            if (_request.RoleID != (int)ERole.Admin)
            {
                response.Message = "Admin only can delete the booking";
                return response;
            }

            currContract.IsDeleted = true;
            currContract.LastModifiedAt = DateTime.UtcNow;
            currContract.LastModifiedBy = _request.UserID;
            currContract.DeletedAt = DateTime.UtcNow;
            currContract.DeletedBy = _request.UserID;

            _request.Context.SaveChanges();

            response.Success = true;
            response.Data = true;
            response.Message = $"Booking #{currContract.ContractId} deleted successfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}