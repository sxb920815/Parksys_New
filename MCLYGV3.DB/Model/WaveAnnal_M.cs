using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MCLYGV3.DB
{
    [Table("WaveAnnal")]
    public  class M_WaveAnnal
    {
        [Key]
        public long WaveAnnalId { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 微波卡号,2.4G卡
        /// </summary>
        [MaxLength(32),Required]
        public string WaveCardId { get; set; }

        [MaxLength(256)]
        public string Note { get; set; }

    }
}
