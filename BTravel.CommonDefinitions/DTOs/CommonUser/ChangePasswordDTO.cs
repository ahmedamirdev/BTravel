using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.DTOs.CommonUser
{
    public class ChangePasswordDTO
    {
        public string  OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public int CommonUserId { get; set; }
    }
}
