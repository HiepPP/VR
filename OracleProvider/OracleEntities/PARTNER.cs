namespace OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PARTNER")]
    public partial class PARTNER
    {
        public double ID { get; set; }

        [StringLength(120)]
        public string NAME { get; set; }

        [StringLength(120)]
        public string ADDRESS { get; set; }

        [StringLength(20)]
        public string TAXCODE { get; set; }

        [StringLength(20)]
        public string TEL { get; set; }

        [StringLength(20)]
        public string FAX { get; set; }

        public DateTime? CREATED_ON { get; set; }

        [StringLength(80)]
        public string CREATED_BY { get; set; }

        [StringLength(20)]
        public string TYPE { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        [StringLength(4000)]
        public string TRK_LIST_IMP { get; set; }

        [StringLength(1)]
        public string IS_TRK_IMP { get; set; }
    }
}
