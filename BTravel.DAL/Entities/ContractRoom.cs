using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;

namespace BTravel.DAL.Entities
{
    public class ContractRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractRoomId { get; set; }

        public string RoomType { get; set; }

        public string Names { get; set; }

        public DateTime CheckIn { get; set; }

        public DateTime CheckOut { get; set; }

        public int NumOfNights { get; set; }

        public string RoomAmenities { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime DeletedAt { get; set; }

        public int DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
        
        public int ContractId { get; set; }

        [ForeignKey("ContractId")]
        public virtual Contract Contract { get; set; }
    }
}