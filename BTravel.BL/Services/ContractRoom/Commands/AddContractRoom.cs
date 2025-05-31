using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.ContractRoom.Commands
{
    public class AddContractRoom : BaseService
    {
        private readonly BaseRequest _request;

        public AddContractRoom(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<int> Add(ContractRoomAddDTO model)
        {
            var response = new BaseResponse<int>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
           
            var newContractRoom = new DAL.Entities.ContractRoom();

            newContractRoom.RoomType = model.RoomType;
            newContractRoom.Names = model.Names;
            newContractRoom.CheckIn = model.CheckIn;
            newContractRoom.CheckOut = model.CheckOut;
            newContractRoom.NumOfNights = model.NumOfNights;
            newContractRoom.RoomAmenities = model.RoomAmenities;
            newContractRoom.Comment = model.Comment;
            newContractRoom.Deadline = model.Deadline;

            newContractRoom.ContractId = model.ContractId;

            newContractRoom.CreatedAt = DateTime.UtcNow;
            newContractRoom.CreatedBy = _request.UserID;
            newContractRoom.IsActive = true;
            newContractRoom.IsDeleted = false;

            _request.Context.ContractRooms.Add(newContractRoom);
            _request.Context.SaveChanges();

            response.Data = newContractRoom.ContractRoomId;

            response.Message = "New Room Added Successfully";
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}