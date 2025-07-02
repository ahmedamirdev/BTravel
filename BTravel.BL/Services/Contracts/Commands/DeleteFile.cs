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
    public class DeleteFile : BaseService
    {
        private readonly BaseRequest _request;

        public DeleteFile(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<int> Delete(int Id)
        {
            var response = new BaseResponse<int>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var currFile = _request.Context.ContractFiles.FirstOrDefault(u => u.ContractFileID == Id && !u.IsDeleted);
            if (currFile == null)
            {
                response.Message = "Invalid BookingFileID";
                return response;
            }

            if (_request.RoleID != (int)ERole.Admin)
            {
                response.Message = "Admin only can delete the file";
                return response;
            }

            currFile.IsDeleted = true;
            currFile.DeletedAt = DateTime.UtcNow;
            currFile.DeletedBy = _request.UserID;

            _request.Context.SaveChanges();

            response.Success = true;
            response.Data = currFile.ContractId;
            response.Message = $"File #{currFile.ContractFileID} deleted successfully";
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}