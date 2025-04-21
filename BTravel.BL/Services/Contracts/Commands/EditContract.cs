using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.CommonUser.Commands;
using BTravel.BL.Services.ContractRoom.Commands;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.Contracts.Commands
{
    public class EditContract : BaseService
    {
        private readonly BaseRequest _request;

        public EditContract(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<ContractDTO> Edit(ContractEditDTO model)
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
            if (model.ContractId <= 0 || string.IsNullOrWhiteSpace(model.HotelName) || model.NoOfRooms <= 0 || model.NoOfNights <= 0 || model.RatePerNight <= 0)
            {
                response.Message = "ContractId, Hotel Name,No Of Rooms,No Of Nights or Rate Per Night is empty";
                return response;
            }
            if (_request.RoleID != (int)ERole.Admin)
            {
                response.Message = "You must be Admin to edit this contract";
                return response;
            }

            var currContract = _request.Context.Contracts.FirstOrDefault(c => !c.IsDeleted && c.ContractId == model.ContractId);
            if (currContract == null)
            {
                response.Message = "Invalid ContractId";
                return response;
            }
            if (currContract.StatusId == (int)EContractStatus.Sigend)
            {
                response.Message = "Contract has been signed and can not be updated";
                return response;
            }

            // currContract.StatusId = (int)EContractStatus.Pending; //*** Ask Mahmoud ??
            currContract.HotelName = model.HotelName;
            currContract.NoOfRooms = model.NoOfRooms;
            currContract.NoOfNights = model.NoOfNights;
            currContract.RatePerNight = model.RatePerNight;
            currContract.TaxPerc = model.TaxPerc;

            currContract.SubTotal = model.RatePerNight * model.NoOfNights;
            currContract.Total = ((currContract.SubTotal * model.TaxPerc) / 100) + currContract.SubTotal;

            _request.Context.SaveChanges();

            //*** Delete old rooms
            var currRooms = _request.Context.ContractRooms.Where(u => u.ContractId == currContract.ContractId && !u.IsDeleted).ToList();
            if (currRooms != null)
            {
                for (int i = 0; i < currRooms.Count; i++)
                {
                    currRooms[i].IsDeleted = true;
                }
                _request.Context.SaveChanges();
            }

            //*** Add new rooms
            if (model.Rooms != null)
            {
                if (model.Rooms.Count > 0)
                {
                    for (int i = 0; i < model.Rooms.Count; i++)
                    {
                        model.Rooms[i].ContractId = currContract.ContractId;

                        var addRoomQuery = new AddContractRoom(_request).Add(model.Rooms[i]);
                    }
                }
            }

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Message = $"Contract #{currContract.ContractId} has been successfully updated";

            return response;
        }
    }
}