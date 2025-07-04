using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.Contracts.Queries;
using BTravel.BL.Services.Mail;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.EntityFrameworkCore;

namespace BTravel.BL.Services.ContractRoom.Commands
{
    public class EditRoomNames : BaseService
    {
        private readonly BaseRequest _request;

        public EditRoomNames(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractDTO> Edit(ContractRoomDTO model)
        {
            var response = new BaseResponse<ContractDTO>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            //*** Validations
            if (model == null)
            {
                response.Message = "Model data is empty";
                return response;
            }
            if (model.ContractRoomId <= 0)
            {
                response.Message = "Invalid RoomId";
                return response;
            }
            var CurrRoom = _request.Context.ContractRooms.Include(r => r.Contract).FirstOrDefault(c => !c.IsDeleted && c.ContractRoomId == model.ContractRoomId);
            if (CurrRoom == null)
            {
                response.Message = "Invalid RoomId";
                return response;
            }
            if (CurrRoom.Contract.StatusId == (int)EContractStatus.Sigend)
            {
                response.Message = "Contract has been signed and can not be updated";
                return response;
            }

            CurrRoom.Names = model.Names;

            CurrRoom.LastModifiedAt = DateTime.UtcNow;
            CurrRoom.LastModifiedBy = _request.UserID;

            _request.Context.SaveChanges();
            
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = $"Room details has been successfully updated";

            return response;
        }
    }
}