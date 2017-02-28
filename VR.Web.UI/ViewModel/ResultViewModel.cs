using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VR.DAL.Model;

namespace VR.Web.UI.ViewModel
{
    public class ResultViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int STT { get; set; }
        [Display(Name ="Truck No.\n(số đầu kéo)")]
        public string TruckNo { get; set; }
        [Display(Name = "Chassic No.\n(số re-mooc)")]
        public string ChassicNo { get; set; }
        [Display(Name = "Consignee\n(chủ hàng)")]
        public string Consignee { get; set; }
        [Display(Name ="Driver Name\n(Tên tài xế)")]
        public string DriverName { get; set; }
        [Display(Name = "ID\n(Số CMND)")]
        public string DriverId { get; set; }
        [Display(Name = "Kết quả lấy thông tin")]
        public string Result { get; set; }

        public TruckStatus TruckStatus { get; set; }
        
        public virtual ListResultViewModel ListResultViewModel { get; set; }
    }
}