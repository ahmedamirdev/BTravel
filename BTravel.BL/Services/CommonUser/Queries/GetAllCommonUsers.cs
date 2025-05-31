using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Helpers;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.EntityFrameworkCore;

namespace BTravel.BL.Services.CommonUser.Queries
{
    public class GetAllCommonUsers : BaseService
    {
        private readonly BaseRequest _request;

        public GetAllCommonUsers(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<IEnumerable<CommonUserDTO>> GetAll(string search)
        {
            var response = new BaseResponse<IEnumerable<CommonUserDTO>>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var query = _request.Context.CommonUsers
                        .Include(c => c.Role)
                        .Where(c => !c.IsDeleted)
                        .OrderByDescending(f => f.CommonUserId)
                        .Select(c => new CommonUserDTO
                        {
                            CommonUserId = c.CommonUserId,
                            FullName = c.FullName,
                            PhoneNumber = c.PhoneNumber,
                            PrimaryMail = c.PrimaryMail,
                            IsPrimaryMailVerified = c.IsPrimaryMailVerified,
                            CreatedAt = c.CreatedAt.ConvertUtcToCairoTime(),
                            ImageUrl = Constants.BaseUrl + c.ImageUrl,
                            CompanyName = c.CompanyName,

                            DefaultSignatureUrl = Constants.BaseUrl + c.DefaultSignatureUrl,

                            RoleId = c.RoleId,
                            RoleName = c.Role.Name,

                            IsActive = c.IsActive,
                        });

            //query = ApplyFilter(query, model.Filter);

            query = ApplySearch(query, search);

            response.TotalCount = query.Count();
            response.PageIndex = _request.PageIndex;
            response.PageSize = _request.PageSize == 0 ? Constants.defaultPageSize : _request.PageSize;
            response.TotalPages = (int)Math.Ceiling(response.TotalCount / (double)response.PageSize);

            var dto = ApplyPaging(query, _request.PageSize, _request.PageIndex);

            response.Data = dto.ToList();
            response.Success = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;

            return response;
        }

        //private static IQueryable<ContractDTO> ApplyFilter(IQueryable<ContractDTO> query, ContractDTO filterDTO)
        //{
        //    if (filterDTO == null)
        //        return query;

        //    if (filterDTO.BusinessId > 0)
        //    {
        //        query = query.Where(q => q.Business.Id == (int)filterDTO.BusinessId);
        //    }
        //    if (filterDTO.CommonUserId > 0)
        //    {
        //        query = query.Where(q => q.User.CommonUserId == filterDTO.CommonUserId);
        //    }
        //    if (filterDTO.ActionId > 0)
        //    {
        //        query = query.Where(q => q.ActionId == filterDTO.ActionId);
        //    }

        //    return query;
        //}

        private static IQueryable<CommonUserDTO> ApplySearch(IQueryable<CommonUserDTO> query, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                query = query.Where(x => x.FullName.ToLower().Contains(search)
                                      || x.PhoneNumber.ToLower().Contains(search)
                                      || x.PrimaryMail.ToLower().Contains(search));
            }

            return query;
        }
    }
}