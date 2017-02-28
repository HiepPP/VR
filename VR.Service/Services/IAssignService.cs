using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Model;

namespace VR.Service.Services
{
    public interface IAssignService
    {
        bool AssignTrkToVoy(string trkNo, string voyCode, int partnerId);
    }
}
