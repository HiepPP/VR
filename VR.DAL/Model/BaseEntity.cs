using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VR.DAL.Model
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Created = DateTime.Now;
            IsDeleted = false;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    }
}
