namespace VR.DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    ///
    /// <summary>
    /// State of file imported
    /// </summary>
    /// 
    public enum ImportedFileState
    {
        Done = 1, //Finished import
        WrongExtension = 2, //Wrong Extionson
        Existed = 3, //Already existed
        NotFinished = 4, //Not finished import
        NotEnoughParams = 5, //Not enough data
        ContentBlank = 6, //File's content is blank
        ContentSizeWrong = 7 //File's content is blank
    }
    [Table("FileExcel")]
    public partial class FileExcel: BaseEntity
    {
        public string UserName { get; set; }
        public int? CustomerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateImport { get; set; }

        [StringLength(50)]
        public string ExcelName { get; set; }
        public int RowTotal { get; set; }
        public bool IsDone { get; set; }
        public string VoyCode { get; set; }
        public virtual ICollection<ExcelDetail> ExcelDetails { get; set; }
    }
}
