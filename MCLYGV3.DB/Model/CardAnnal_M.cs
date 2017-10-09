using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{

    /// <summary>
    /// 车牌识别记录表
    /// </summary>
    [Table("CardAnnal")]
    public class M_CardAnnal
    {
        [Key]
        public long CardAnnalId { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        public long? AreaId { get; set; }

        public long? EquipmentId { get; set; }

        [MaxLength(32)]
        public string State { get; set; }

        [MaxLength(32)]
        public string License { get; set; }

        [MaxLength(128)]
        public string ImagePath { get; set; }

        /// <summary>
        /// 0表示不违章，1表示违章
        /// </summary>
        [Required, DefaultValue(0)]
        public int IsBreak { get; set; }

        [MaxLength(256)]
        public string Note { get; set; }
    }
}
