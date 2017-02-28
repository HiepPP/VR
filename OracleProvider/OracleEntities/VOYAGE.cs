namespace OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VOYAGE")]
    public partial class VOYAGE
    {
        public double ID { get; set; }

        [StringLength(80)]
        public string NAME { get; set; }

        [StringLength(10)]
        public string CALL_SEQ { get; set; }

        public short? CALL_YEAR { get; set; }

        public double? DISCHARGE_TON { get; set; }

        public double? LOAD_TON { get; set; }

        public DateTime? ETB { get; set; }

        public DateTime? ATB { get; set; }

        public DateTime? ETD { get; set; }

        public DateTime? ATD { get; set; }

        [StringLength(20)]
        public string DEC_NO { get; set; }

        [StringLength(20)]
        public string BILL_NO { get; set; }

        public DateTime? CREATED_ON { get; set; }

        [StringLength(80)]
        public string CREATED_BY { get; set; }

        [StringLength(15)]
        public string CODE { get; set; }

        [StringLength(20)]
        public string SERVICELANE { get; set; }

        [StringLength(1)]
        public string BAR_VSL { get; set; }

        public decimal? GRT { get; set; }

        public int? LOA { get; set; }

        [StringLength(20)]
        public string WHARFMARK { get; set; }

        public decimal? DRAFT_ARRIVE { get; set; }

        public decimal? DRAFT_AFTARRIVE { get; set; }

        public decimal? DRAFT_DEPART { get; set; }

        public decimal? DRAFT_AFTDEPART { get; set; }

        [StringLength(20)]
        public string EQUIPMENT { get; set; }

        [StringLength(50)]
        public string AGENT { get; set; }

        [StringLength(80)]
        public string CONTACT { get; set; }

        [StringLength(80)]
        public string NOTES { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? START_OPER_TIME { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? END_OPER_TIME { get; set; }

        [StringLength(200)]
        public string CAPTAIN { get; set; }

        public decimal? DWT { get; set; }

        [StringLength(3)]
        public string VSL_MOD { get; set; }

        [StringLength(1)]
        public string USE_WB { get; set; }

        [StringLength(1)]
        public string IS_CONTAINER { get; set; }

        [StringLength(1)]
        public string TML_HOLD { get; set; }
    }
}
