using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{
    [Table("AreaAndEqu")]
    public class M_AreaAndEqu
    {
        [Key]
        public virtual long Id { get; set; }

        
        public virtual long AreaId { get; set; }

        public virtual long EquipmentId { get; set; }

        public virtual string State { get;set; }


    }
}
