using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Helpers;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Enums;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;
using Microsoft.EntityFrameworkCore;

namespace BTravel.BL.Services.Contracts.Queries
{
    public class GetAllContracts : BaseService
    {
        private readonly BaseRequest _request;

        public GetAllContracts(BaseRequest request)
        {
            _request = request;
        }

        public BaseResponse<IEnumerable<ContractDTO>> GetAll(string search)
        {
            var response = new BaseResponse<IEnumerable<ContractDTO>>();
            response.Success = false;
            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            
            var query = _request.Context.Contracts
                        .Include(c => c.CommonUser)
                        .Include(c => c.Rooms)
                        .Include(c => c.Files)
                        .Where(c => !c.IsDeleted)
                        .OrderByDescending(f => f.ContractId)
                        .Select(c => new ContractDTO
                        {
                            ContractId = c.ContractId,
                            CreatedAt = c.CreatedAt.ConvertUtcToCairoTime(),
                            CreatedBy = c.CreatedBy,
                            LastModifiedAt = c.LastModifiedAt.ConvertUtcToCairoTime(),
                            LastModifiedBy = c.LastModifiedBy,
                            IsSigned = c.IsSigned,
                            SignedAt = c.SignedAt.HasValue ? c.SignedAt.Value.ConvertUtcToCairoTime() : null,
                            SignatureUrl = Constants.BaseUrl + c.SignatureUrl,
                            StatusId = c.StatusId,
                            IsViewed = c.IsViewed,
                            ViewedAt = c.ViewedAt.HasValue ? c.ViewedAt.Value.ConvertUtcToCairoTime() : null,

                            HotelName = c.HotelName,
                            NoOfRooms = c.NoOfRooms,
                            NoOfNights = c.NoOfNights,
                            RatePerNight = c.RatePerNight,
                            TaxPerc = c.TaxPerc,
                            SubTotal = c.SubTotal,
                            Total = c.Total,

                            CardNumber = AESEncryptionHelper.Decrypt(c.CardNumber),
                            NameOnCreditCard = AESEncryptionHelper.Decrypt(c.NameOnCreditCard),
                            CardCVC = AESEncryptionHelper.Decrypt(c.CardCVC),
                            CardExpDate = AESEncryptionHelper.Decrypt(c.CardExpDate),

                            BillingAddress = c.BillingAddress,
                            PostalCode = c.PostalCode,

                            CommonUser = new CommonDefinitions.DTOs.CommonUser.CommonUserDTO
                            {
                                CommonUserId = c.CommonUserId,
                                FullName = c.CommonUser.FullName,
                                PhoneNumber = c.CommonUser.PhoneNumber,
                                PrimaryMail = c.CommonUser.PrimaryMail,
                                IsPrimaryMailVerified = c.CommonUser.IsPrimaryMailVerified,
                                CreatedAt = c.CommonUser.CreatedAt.ConvertUtcToCairoTime(),
                                ImageUrl = Constants.BaseUrl + c.CommonUser.ImageUrl,
                                CompanyName = c.CommonUser.CompanyName,

                                DefaultSignatureUrl = Constants.BaseUrl + c.CommonUser.DefaultSignatureUrl,

                                RoleId = c.CommonUser.RoleId,
                                RoleName = c.CommonUser.Role.Name,
                            },

                            Rooms = c.Rooms.Where(r => !r.IsDeleted)
                                    .Select(r => new ContractRoomDTO
                                    {
                                        ContractRoomId = r.ContractRoomId,
                                        RoomType = r.RoomType,
                                        Names = r.Names,
                                        CheckIn = r.CheckIn,
                                        CheckOut = r.CheckOut,
                                        NumOfNights = r.NumOfNights,
                                        RoomAmenities = r.RoomAmenities,
                                        Comment = r.Comment,
                                        Deadline = r.Deadline,
                                        CreatedAt = r.CreatedAt.ConvertUtcToCairoTime(),
                                        CreatedBy = r.CreatedBy,
                                        ContractId = r.ContractId,
                                    }),
                        });

            //query = ApplyFilter(query, model.Filter);

            query = ApplySearch(query, search);

            response.Search = search;

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

        private static IQueryable<ContractDTO> ApplySearch(IQueryable<ContractDTO> query, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();

                if (search == "pending")
                {
                    query = query.Where(x => x.StatusId == (int)EContractStatus.Pending);
                }
                else if (search == "opened")
                {
                    query = query.Where(x => x.StatusId == (int)EContractStatus.Opened);
                }
                else if (search == "signed")
                {
                    query = query.Where(x => x.StatusId == (int)EContractStatus.Sigend);
                }
                else
                {
                    query = query.Where(x => x.HotelName.ToLower().Contains(search)
                                          || x.CommonUser.FullName.ToLower().Contains(search)
                                          || x.CommonUser.PrimaryMail.ToLower().Contains(search)
                                          || x.ContractId.ToString().Contains(search));
                }
            }

            return query;
        }
    }
}