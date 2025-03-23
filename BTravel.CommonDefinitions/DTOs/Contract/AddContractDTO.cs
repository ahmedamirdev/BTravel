using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.DTOs.Contract
{
    public class AddContractDTO
    {
        public string HotelName { get; set; }

        public int NoOfRooms { get; set; }

        public int NoOfNights { get; set; }

        public decimal RatePerNight { get; set; }   
        public string PrimaryMail { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

    }
}
