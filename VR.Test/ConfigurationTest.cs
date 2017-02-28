using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VR.Service.Services;
using System.Configuration;

namespace VR.Test
{
    [TestClass]
    public class ConfigurationTest
    {
        Mock<ConfigurationService> _ConfigurationService;
        VR.DAL.Model.Configuration config;
        VR.DAL.Model.Configuration nullconfig;
        [TestInitialize]
        public void Create()
        {
            _ConfigurationService = new Mock<ConfigurationService>();
            config = new VR.DAL.Model.Configuration
            {
                WebsiteAddress = "www.vr.org.vn",
                TruckType = "txtLoaiPT",
                NetWeight = "txtTuTrongTK",
                MaxSeat = "txtSoCho",
                DateCheck = "txtNgayKD",
                AxleNo = "txtCdCsCtBx",
                WeightLimit = "txtTaiTrongGT",
                GrossWeight = "txtTrLgToanBoGT",
                TowedWeight = "txtTrLgMoocCP",
                GCNDate = "txtHanKDToi",
                GCNStamp = "txtSoTemGCN"
            };
            nullconfig = null;
        }
        /// <summary>
        /// Cleanup after execute tests
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _ConfigurationService = null;
        }
        /// <summary>
        /// Import Excel Success
        /// </summary>
        //Save config success
        [TestMethod]
        public void ConfigSaveSuccess()
        {
            VR.DAL.Model.Configuration result = _ConfigurationService.Object.Insert(config);
            Assert.IsNotNull(result);
        }
        //Save config unsuccess
        [TestMethod]
        public void ConfigSaveUnSuccess()
        {
            VR.DAL.Model.Configuration result = _ConfigurationService.Object.Insert(nullconfig);
            Assert.IsNull(result);
        }
    }
}
