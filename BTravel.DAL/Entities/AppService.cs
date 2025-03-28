using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.DAL.Entities
{
    public class AppService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppServiceId { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public string ViewName { get; set; }

        public string Description { get; set; }

        public string ClassName { get; set; }

        public virtual ICollection<RoleAppService> RoleAppServices { get; set; }  
    }
}