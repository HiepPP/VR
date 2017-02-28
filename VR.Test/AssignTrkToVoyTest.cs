using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VR.Service.Services;
using Moq;

namespace VR.Test
{
    /// <summary>
    /// Test AssignTrkToVoy function of AssignService
    /// </summary>
    [TestClass]
    public class AssignTrkToVoyTest
    {
        private Mock<AssignService> _assignService;
        private string strTrkNoNormal;
        private string strTrkNoAbnormal;
        private string strVoyCodeValid;
        private string strVoyCodeInvalid;
        private int? intPartnerId;
        /// <summary>
        /// Initialize
        /// </summary>
        [TestInitialize]
        public void Create()
        {
            _assignService = new Mock<AssignService>();
            strTrkNoNormal = "1234567";
            strVoyCodeValid = "GDFN";
            intPartnerId = 22;
            strVoyCodeInvalid = "GGGG";
            strTrkNoAbnormal = null;
        }
        /// <summary>
        /// Cleanup after execute tests
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _assignService = null;
            strTrkNoNormal = string.Empty;
            strVoyCodeValid = null;
            intPartnerId = null;
        }
        /// <summary>
        /// Normal value to check if function run right
        /// </summary>
        [TestMethod]
        public void AssignTrkToVoy_Success()
        {               
            bool result = _assignService.Object.AssignTrkToVoy(strTrkNoNormal, strVoyCodeValid, intPartnerId.Value);
            Assert.IsTrue(result);
        }
        /// <summary>
        /// Abnormal Voyage id
        /// </summary>
        [TestMethod]
        public void AssignTrkToVoy_FailVoyId()
        {         
            bool result = _assignService.Object.AssignTrkToVoy(strTrkNoNormal, strVoyCodeInvalid, intPartnerId.Value);
            Assert.IsFalse(result);
        }
        /// <summary>
        /// Null Voyage id
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AssignTrkToVoy_NullPartner()
        {                      
            intPartnerId = null;
            bool result = _assignService.Object.AssignTrkToVoy(strTrkNoNormal, strVoyCodeInvalid, intPartnerId.Value);           
        }
        /// <summary>
        /// Null Truck No
        /// </summary>
        [TestMethod]
        public void AssignTrkToVoy_FailTrKNo()
        {
            bool result = _assignService.Object.AssignTrkToVoy(strTrkNoAbnormal, strVoyCodeValid, intPartnerId.Value);
            Assert.IsFalse(result);
        }
    }
}
