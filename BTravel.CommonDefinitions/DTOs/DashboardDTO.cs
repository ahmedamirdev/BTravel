using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTravel.CommonDefinitions.DTOs
{
    public class DashboardDTO
    {
        public int TotalNumOfCustomers { get; set; }
        public int PendingContracts { get; set; }
        public int OpenedContracts { get; set; }
        public int SignedContracts { get; set; }

        public decimal TotalSales { get; set; }
    }
}