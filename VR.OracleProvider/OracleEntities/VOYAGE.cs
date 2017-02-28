namespace VR.OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VOYAGE")]
    public partial class VOYAGE: OracleBaseEntity
    { 
        [StringLength(80)]
        public string NAME { get; set; }

        public DateTime? ETB { get; set; }
        public DateTime? ATB { get; set; }

        public DateTime? ATD { get; set; }

        [StringLength(20)]
        public string BILL_NO { get; set; }
      
        [StringLength(15)]
        public string CODE { get; set; }

    
    }
}
