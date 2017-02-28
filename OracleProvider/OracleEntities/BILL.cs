namespace OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BILL")]
    public partial class BILL
    {
        public double ID { get; set; }

        [StringLength(20)]
        public string BILL_NO { get; set; }

        [StringLength(80)]
        public string REMARK { get; set; }

        public double? VOY_ID { get; set; }

        public DateTime? CREATED_ON { get; set; }

        [StringLength(80)]
        public string CREATED_BY { get; set; }

        public double? PLAN_WEI { get; set; }

        public double? HANDLED_WEI { get; set; }

        public double? BAL_WEI { get; set; }

        public double? PNR_ID { get; set; }

        public long? PLANNED_TOTAL { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        public double? CGO_ID { get; set; }
    }
}
