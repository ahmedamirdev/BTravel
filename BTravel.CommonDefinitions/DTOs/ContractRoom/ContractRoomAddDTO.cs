using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.DTOs.ContractRoom
{
    public class ContractRoomAddDTO
    {
        public int ContractId { get; set; }

        public string RoomType { get; set; }
        public string Names { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NumOfNights { get; set; }
        public string RoomAmenities { get; set; }

        public string? Comment { get; set; }
        public DateTime Deadline { get; set; }
    }
}