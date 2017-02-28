namespace VR.DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public enum TruckStatus
    {
        [Description("Not yet check")]
        NotYetCheck = 1,
        [Description("Error while checking")]
        ErrorWhileChecking = 2,
        [Description("Get Info successfully but truck is not valid")]
        GetInfoButNotValid = 3,
        [Description("Get Info successfully and truck is valid")]
        GetInfoAndValid = 4,
        [Description("Had info in database and still valid")]
        HaveInfoAndValid = 5,
        [Description("On Processing")]
        OnProcessing = 6,
    }

    [Table("ExcelDetail")]
    public partial class ExcelDetail: BaseEntity
    {
       

        [StringLength(20)]
        public string TruckNo { get; set; }

        [StringLength(20)]
        public string ChassisNo { get; set; } 

        [StringLength(80)]
        public string DriverName { get; set; }

        [StringLength(20)]
        public string DriverId { get; set; }

        [StringLength(20)]
        public string LicenseNo { get; set; }

        public TruckStatus TruckStatus { get; set; }

       
        [ForeignKey("FileExcel")]
        public int FileExcelId { get; set; }
        public virtual FileExcel FileExcel { get; set; }
      
        public int? LinkedId { get; set; }
    }
}
