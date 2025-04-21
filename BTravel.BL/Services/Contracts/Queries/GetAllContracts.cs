using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.BL.Services.Security.Encryption;
using BTravel.CommonDefinitions;
using BTravel.CommonDefinitions.DTOs.Contract;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using BTravel.CommonDefinitions.Requests;
using BTravel.CommonDefinitions.Responses;

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

            if (string.IsNullOrWhiteSpace(search))
                search = "";
            
            search = search.ToLower();

            var query = _request.Context.Contracts.Where(c => !c.IsDeleted)
                        .Where(x => x.HotelName.ToLower().Contains(search) 
                            || x.CommonUser.FullName.ToLower().Contains(search)
                            || x.CommonUser.PrimaryMail.ToLower().Contains(search)
                            || x.ContractId.ToString().Contains(search))
                        .OrderByDescending(f => f.ContractId)
                        .Select(c => new ContractDTO
                        {
                            ContractId = c.ContractId,
                            CreatedAt = c.CreatedAt.AddHours(2),
                            CreatedBy = c.CreatedBy,
                            LastModifiedAt = c.LastModifiedAt.AddHours(2),
                            LastModifiedBy = c.LastModifiedBy,
                            SignedAt = c.SignedAt,
                            SignatureUrl = c.SignatureUrl,
                            StatusId = c.StatusId,

                            HotelName = c.HotelName,
                            NoOfRooms = c.NoOfRooms,
                            NoOfNights = c.NoOfNights,
                            RatePerNight = c.RatePerNight,
                            TaxPerc = c.TaxPerc,
                            SubTotal = c.SubTotal,
                            Total = c.Total,

                            CommonUser = new CommonDefinitions.DTOs.CommonUser.CommonUserDTO
                            {
                                CommonUserId = c.CommonUserId,
                                FullName = c.CommonUser.FullName,
                                PhoneNumber = c.CommonUser.PhoneNumber,
                                PrimaryMail = c.CommonUser.PrimaryMail,
                                IsPrimaryMailVerified = c.CommonUser.IsPrimaryMailVerified,
                                CreatedAt = c.CommonUser.CreatedAt.AddHours(2),
                                ImageUrl = c.CommonUser.ImageUrl,
                                CompanyName = c.CommonUser.CompanyName,

                                CardNumber = AESEncryptionHelper.Decrypt(c.CommonUser.CardNumber),
                                NameOnCreditCard = AESEncryptionHelper.Decrypt(c.CommonUser.NameOnCreditCard),
                                CardCVC = AESEncryptionHelper.Decrypt(c.CommonUser.CardCVC),
                                CardExpDate = AESEncryptionHelper.Decrypt(c.CommonUser.CardExpDate),

                                BillingAddress = c.CommonUser.BillingAddress,
                                PostalCode = c.CommonUser.PostalCode,
                                DefaultSignatureUrl = c.CommonUser.DefaultSignatureUrl,

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
                                        CreatedAt = r.CreatedAt.AddHours(2),
                                        CreatedBy = r.CreatedBy,
                                        ContractId = r.ContractId,
                                    }),
                        });

            //query = ApplyFilter(query, model.Filter);

            //query = ApplySearch(query, search);

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

                query = query.Where(x => x.HotelName.ToLower().Contains(search));
            }

            return query;
        }
    }
}