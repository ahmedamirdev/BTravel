using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.DTOs.ContractRoom
{
    public class ContractRoomDTO
    {
        public int ContractRoomId { get; set; }

        public string RoomType { get; set; }
        public string Names { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NumOfNights { get; set; }
        public string RoomAmenities { get; set; }

        public string? Comment { get; set; }
        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }

        public int ContractId { get; set; }
    }
}