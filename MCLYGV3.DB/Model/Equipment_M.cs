using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{
    [Table("Equipment")]
    public  class M_Equipment
    {
        [Key]
        public long EquipmentId { get; set; }

        [Required,MaxLength(32)]
        public string EquipmentName { get; set; }

        [Required,MaxLength(32)]
        public string IP { get; set; }

        [Required,MaxLength(32)]
        public string Serialno { get; set; }
    }
}
