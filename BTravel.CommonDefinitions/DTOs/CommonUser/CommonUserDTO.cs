using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.DTOs.CommonUser
{
    public class CommonUserDTO
    {
        public int CommonUserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string PrimaryMail { get; set; }
        public bool IsPrimaryMailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
        public string? CompanyName { get; set; }
        
        public string? DefaultSignatureUrl { get; set; }
        public bool IsActive { get; set; }

        public int RoleId { get; set; } // 1 = Admin , 2 = client
        public string RoleName { get; set; }
    }
}