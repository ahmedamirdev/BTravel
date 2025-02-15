using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.DAL.Entities
{
    public class CommonUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommonUserId { get; set; }

        [Required]
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string PrimaryMail { get; set; }

        public bool IsPrimaryMailVerified { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime DeletedAt { get; set; }

        public int DeletedBy { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string Password { get; set; }

        public string ImageUrl { get; set; }

        public string CompanyName { get; set; }

        public string CardNumber { get; set; }

        public DateTime CardExpDate { get; set; }

        public string CardCVC { get; set; }

        public string NameOnCreditCard { get; set; }

        public string BillingAddress { get; set; }

        public string PostalCode { get; set; }

        public string DefaultSignatureUrl { get; set; }

        public ICollection<Contract> Contracts { get; set; }

    }
}
