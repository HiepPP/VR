namespace VR.OracleProvider.OracleEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PARTNER")]
    public partial class PARTNER: OracleBaseEntity
    {
        //public decimal ID { get; set; }
        [StringLength(120)]
        public string NAME { get; set; }
        [StringLength(120)]
        public string ADDRESS { get; set; }
        [StringLength(20)]
        public string TEL { get; set; }
 
     
        [StringLength(20)]
        public string TYPEPRN { get; set; }



    }
}
