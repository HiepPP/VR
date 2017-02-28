using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Repository;
using VR.DAL.Model;
namespace VR.Service.Services
{
    public class ConfigurationService: IConfigurationService
    {
        private readonly IConfigurationRepository _configrepo = new ConfigurationRepository();
        public Configuration Insert(Configuration config)
        {
            return _configrepo.Insert(config);
        }
        public IEnumerable<Configuration> GetAll()
        {
            return _configrepo.GetAll();
        }

        public Configuration Update(Configuration config)
        {
            return _configrepo.Update(config);
        }
        public Configuration GetById(int id)
        {
            return _configrepo.GetById(id);
        }
    }
}
