using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.DAL.Entities
{
    public class ContractFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractFileID { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime DeletedAt { get; set; }

        public int DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public int ContractId { get; set; }

        [ForeignKey("ContractId")]
        public virtual Contract Contract { get; set; }

        public string FileUrl { get; set; }
        public string FileName { get; set; }
    }
}