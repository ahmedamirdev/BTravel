using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.ContractRoom;

namespace BTravel.CommonDefinitions.DTOs.Contract
{
    public class ContractEditDTO
    {
        public int ContractId { get; set; }
        public string HotelName { get; set; }
        public int NoOfRooms { get; set; }
        public int NoOfNights { get; set; }
        public decimal RatePerNight { get; set; }
        public decimal TaxPerc { get; set; }

        public string? CardNumber { get; set; }
        public string? CardExpDate { get; set; }
        public string? CardCVC { get; set; }
        public string? NameOnCreditCard { get; set; }
        public string? BillingAddress { get; set; }
        public string? PostalCode { get; set; }

        public int CommonUserId { get; set; }

        public List<ContractRoomAddDTO> Rooms { get; set; }
    }
}