using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VR.OracleProvider.OracleEntities
{
    public class OracleBaseEntity
    {
        public OracleBaseEntity()
        {
            CREATED_ON = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CREATED_ON { get; set; }
    }
}
