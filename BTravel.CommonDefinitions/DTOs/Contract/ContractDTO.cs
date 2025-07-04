using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.ContractRoom;
using Microsoft.AspNetCore.Http;

namespace BTravel.CommonDefinitions.DTOs.Contract
{
    public class ContractDTO
    {
        public int ContractId { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public int LastModifiedBy { get; set; }

        public bool IsSigned { get; set; }
        public DateTime? SignedAt { get; set; }
        public string? SignatureUrl { get; set; }
        public int StatusId { get; set; } // Enum 1 = Pending , 2 = Opened , 3 = Signed 
        public string HotelName { get; set; }
        public int NoOfRooms { get; set; }
        public int NoOfNights { get; set; }
        public decimal RatePerNight { get; set; }
        public decimal TaxPerc { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }

        public string? CardNumber { get; set; }
        public string? CardExpDate { get; set; }
        public string? CardCVC { get; set; }
        public string? NameOnCreditCard { get; set; }
        public string? BillingAddress { get; set; }
        public string? PostalCode { get; set; }

        public CommonUserDTO CommonUser { get; set; }
        public IEnumerable<ContractRoomDTO> Rooms { get; set; }
        public List<ContractRoomDTO> RoomsList { get; set; }
        public IEnumerable<ContractFileDTO> Files { get; set; }

        public DateTime? ViewedAt { get; set; }
        public bool IsViewed { get; set; }

        public string signatureData { get; set; }
    }

    public class ContractFileDTO
    {
        public int FileId { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}