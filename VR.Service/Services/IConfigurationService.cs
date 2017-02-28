using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL;
using VR.DAL.Model;

namespace VR.Service.Services
{
    public interface IConfigurationService
    {
        Configuration Insert(Configuration config);
        IEnumerable<Configuration> GetAll();
        Configuration Update(Configuration config);
        Configuration GetById(int id);
    }
}
