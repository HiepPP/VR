namespace VR.OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("PARTNER_TRUCK")]
    public partial class PARTNER_TRUCK: OracleBaseEntity
    {
        //public decimal ID { get; set; }
        public decimal? PNR_ID { get; set; }
        public decimal? VOY_ID { get; set; }
        public decimal? TRK_ID { get; set; }
        [StringLength(80)]
        public string REMARK { get; set; }
     

    }
}
