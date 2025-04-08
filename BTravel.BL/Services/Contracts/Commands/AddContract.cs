using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.CommonUser.Commands;
using BTravel.BL.Services.ContractRoom.Commands;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.Contracts.Commands
{
    internal class AddContract : BaseService
    {
        private readonly BaseRequest _request;

        public AddContract(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractDTO> Add(ContractAddDTO model)
        {
            var response = new BaseResponse<ContractDTO>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => !u.IsDeleted && u.PrimaryMail.ToLower() == model.CommonUserAddDTO.PrimaryMail.ToLower());
            if (currCommonUser == null)
            {
                model.CommonUserAddDTO.Password = "123";
                model.CommonUserAddDTO.RoleId = (int)ERole.Client;

                var addUserResponse = new AddCommonUser(_request).Add(model.CommonUserAddDTO);

                currCommonUser = _request.Context.CommonUsers.FirstOrDefault(u => u.CommonUserId == addUserResponse.Data);
            }

            DAL.Entities.Contract newContract = new DAL.Entities.Contract();
            newContract.StatusId = (int)EContractStatus.Pending;
            newContract.CreatedAt = DateTime.UtcNow;
            newContract.CreatedBy = _request.UserID;
            newContract.IsDeleted = false;
            newContract.IsActive = true;
            newContract.HotelName = model.HotelName;
            newContract.NoOfRooms = model.NoOfRooms;
            newContract.NoOfNights = model.NoOfNights;
            newContract.RatePerNight = model.RatePerNight;
            newContract.CommonUserId = currCommonUser.CommonUserId;
            newContract.TaxPerc = model.TaxPerc;

            var subTotal = model.RatePerNight * model.NoOfNights;
            newContract.Total = ((subTotal * model.TaxPerc) / 100) + subTotal;

            _request.Context.Contracts.Add(newContract);
            _request.Context.SaveChanges();

            if (model.Rooms.Count > 0)
            {
                for (int i = 0; i < model.Rooms.Count; i++)
                {
                    model.Rooms[i].ContractId = newContract.ContractId;

                    var addRoomQuery = new AddContractRoom(_request).Add(model.Rooms[i]);
                }
            }

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }
    }
}