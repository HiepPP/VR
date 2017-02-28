namespace VR.OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BILL")]
    public partial class BILL: OracleBaseEntity
    {
        //public decimal ID { get; set; }

        [StringLength(20)]
        public string BILL_NO { get; set; }
        [StringLength(80)]
        public string REMARK { get; set; }
        public decimal? VOY_ID { get; set; }
        public decimal? PNR_ID { get; set; }
    }
}
