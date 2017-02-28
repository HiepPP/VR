using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VR.OracleProvider.OracleEntities
{
    [Table("APP_USER")]
    public class APPUSER: OracleBaseEntity
    {
        [StringLength(10)]
        public string CODE { get; set; }
        [StringLength(60)]
        public string USER_NAME { get; set; }
        [StringLength(50)]
        public string FULL_NAME { get; set; }
        [StringLength(255)]
        public string PWD { get; set;}
        [StringLength(1)]
        public string STATUS { get; set; }
       
        public long? ROL_ID { get; set; }     
    }
}
