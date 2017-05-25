using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.BasicSetup.ViewModels;
using Models.Customize.Models;

namespace Models.Customize.ViewModels
{
    
    public partial class Dimension1ViewModel
    {
        public int Dimension1Id { get; set; }
        public int DocTypeId { get; set; }
        public int? ProductTypeId { get; set; }
        public string Dimension1Name { get; set; }
        public Decimal Multiplier { get; set; }
        public int CostCenterId { get; set; }
        public Boolean IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
