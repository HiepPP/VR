namespace VR.DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Configuration")]
    public class Configuration: BaseEntity
    {
        public string WebsiteAddress { get; set; }

        [StringLength(50)]
        public string TruckType { get; set; }

        [StringLength(50)]
        public string NetWeight { get; set; }

        [StringLength(50)]
        public string MaxSeat { get; set; }

        [StringLength(50)]
        public string DateCheck { get; set; }
     
        [StringLength(50)]
        public string GCNStamp { get; set; }

        [StringLength(50)]
        public string WeightLimit { get; set; }

        [StringLength(50)]
        public string GrossWeight { get; set; }

        [StringLength(50)]
        public string TowedWeight { get; set; }

        [StringLength(50)]
        public string GCNDate { get; set; }

        [StringLength(50)]
        public string AxleNo { get; set; }
    }
}
