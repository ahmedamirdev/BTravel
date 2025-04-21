using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.CommonDefinitions.DTOs.CommonUser;
using BTravel.CommonDefinitions.DTOs.Contract;

namespace BTravel.CommonDefinitions.DTOs
{
    public class DashboardDTO
    {
        public int TotalNumOfCustomers { get; set; }
        public int PendingContracts { get; set; }
        public int OpenedContracts { get; set; }
        public int SignedContracts { get; set; }

        public decimal TotalSalesAllTime { get; set; }
        public DateTime FirstContractCreatedAt { get; set; }

        public decimal TotalSalesLastWeek { get; set; }
        public DateTime LastWeekStartAt { get; set; }

        public IEnumerable<ContractDTO> LatestContracts { get; set; } = new List<ContractDTO>();
        public IEnumerable<CommonUserDTO> LatestUsers { get; set; } = new List<CommonUserDTO>();
    }
}