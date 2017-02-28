namespace OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PARTNER_TRUCK
    {
        public double ID { get; set; }

        public double? PNR_ID { get; set; }

        public double? VOY_ID { get; set; }

        public double? TRK_ID { get; set; }

        [StringLength(80)]
        public string REMARK { get; set; }

        public DateTime? CREATED_ON { get; set; }

        [StringLength(80)]
        public string CREATED_BY { get; set; }
    }
}
