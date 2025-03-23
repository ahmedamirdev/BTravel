using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.CommonUser.Commands;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

namespace BTravel.BL.Services.Contracts.Commands
{
    internal class AddContract : BaseService
    {
        private readonly BaseRequest _baseRequest;

        public AddContract(BaseRequest baseRequest)
        {
            _baseRequest = baseRequest;
        }

        public BaseResponse<ContractDTO> Add(AddContractDTO model) 
        {
            var response = new BaseResponse<ContractDTO>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var newCommonUserID = 0;
            // Add new contract to database
            var currCommonUser = _baseRequest.Context.CommonUsers.FirstOrDefault(c => c.PrimaryMail.ToLower() == model.PrimaryMail.ToLower() && !c.IsDeleted );
            if (currCommonUser == null) 
            {
                
                var addUserQuery = new AddCommonUser(_baseRequest);
                var newCommonUser = new AddCommonUserDTO
                {
                    FullName = model.FullName,
                    PrimaryMail = model.PrimaryMail,
                    Password = "123",
                    PhoneNumber = model.PhoneNumber,
                    RoleId = (int)ERole.Client

                };
                var addUserResponse = addUserQuery.Add(newCommonUser);
                if (!addUserResponse.Success)
                {
                    response.Message = addUserResponse.Message;
                    return response;
                }

                newCommonUserID = addUserResponse.Data;
            }
            DAL.Entities.Contract newContract = new DAL.Entities.Contract();

            newContract.CommonUserId = currCommonUser == null ? newCommonUserID : currCommonUser.CommonUserId;

            _baseRequest.Context.Contracts.Add(newContract);
            _baseRequest.Context.SaveChanges();

            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;

        }
      
    }
}
