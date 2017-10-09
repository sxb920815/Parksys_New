using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{
    [Table("Car")]
    public class M_Car
    {
        [Key]
        public long CarId { get; set; }

        [MaxLength(32)]
        public string OwnerName { get; set; }
        
        [MaxLength(32)]
        public string OwnerPhone { get; set; }

        [MaxLength(32),Required]
        public string License { get; set; }

        public long AreaId { get; set; }

        [MaxLength(32)]
        public string Note { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }


    }
}
