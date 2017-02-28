namespace OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRUCK")]
    public partial class TRUCK
    {
        public double ID { get; set; }

        [StringLength(80)]
        public string NAME { get; set; }

        [StringLength(20)]
        public string TRK_NO { get; set; }

        [StringLength(20)]
        public string LICENSE_NO { get; set; }

        [StringLength(80)]
        public string DRIVER_NAME { get; set; }

        [StringLength(20)]
        public string DRIVER_ID { get; set; }

        public double? WEI_LIMIT { get; set; }

        public DateTime? CREATED_ON { get; set; }

        [StringLength(80)]
        public string CREATED_BY { get; set; }

        [StringLength(20)]
        public string REMOOC_NO { get; set; }

        public long? CROSS_WEI_LIMIT { get; set; }

        public long? TRK_WEI_LIMIT { get; set; }

        public long? NET_WEI_LIMIT { get; set; }

        [StringLength(1)]
        public string TRK_TYPE { get; set; }

        public long? KERB_MASS { get; set; }

        public long? TOWED_WEIGHTED { get; set; }

        public long? REMOOC_MASS { get; set; }

        public long? CROSS_OVERWEI { get; set; }

        public long? NET_OVERWEI { get; set; }

        public DateTime? CERT_DUEDATE { get; set; }

        public double? REMOOC_ID { get; set; }

        [StringLength(40)]
        public string CERT_NO { get; set; }

        public double? TRK_AXLE { get; set; }
    }
}
