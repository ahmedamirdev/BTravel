using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.DTOs.CommonUser
{
    public class CommonUserAddDTO
    {
        public int CommonUserId { get; set; }
        public string FullName { get; set; }
        public string PrimaryMail { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string? CardNumber { get; set; }
        public string? CardExpDate { get; set; }
        public string? CardCVC { get; set; }
        public string? NameOnCreditCard { get; set; }
        public string? BillingAddress { get; set; }
        public string? PostalCode { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}