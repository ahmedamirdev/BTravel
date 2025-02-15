using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.DAL.Entities
{
    public class Contract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractId { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime DeletedAt { get; set; }

        public int DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public DateTime SignedAt { get; set; }

        public string SignatureUrl { get; set; }

        public int StatusId { get; set; }

        public string HotelName { get; set; }

        public int NoOfRooms { get; set; }

        public int NoOfNights { get; set; }

        public decimal RatePerNight { get; set; }

        public int CommonUserId { get; set; }
         
        [ForeignKey("CommonUserId")]
        public CommonUser CommonUser { get; set; }
    }
}
