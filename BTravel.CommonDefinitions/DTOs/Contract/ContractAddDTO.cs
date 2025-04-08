using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.ContractRoom;

namespace BTravel.CommonDefinitions.DTOs.Contract
{
    public class ContractAddDTO
    {
        public string HotelName { get; set; }
        public int NoOfRooms { get; set; }
        public int NoOfNights { get; set; }
        public decimal RatePerNight { get; set; }
        public decimal TaxPerc { get; set; }

        public int CommonUserId { get; set; }
        public CommonUserAddDTO CommonUserAddDTO { get; set; }

        public List<ContractRoomAddDTO> Rooms { get; set; }
    }
}