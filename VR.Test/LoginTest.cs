using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VR.OracleProvider.OracleEntities;
using VR.Service.Services;

namespace VR.Test
{
    [TestClass]
    public class LoginTest
    {
        private string _admin;
        private string _user;
        private string _password;
        private string _wronguser;
        private string _wrongpass;
        private string _nulluser;
        private string _nullpass;
        private int _adminrole;
        private int _userrole;
        Mock<OraAppUserService> _oraAppUserService;
        [TestInitialize]
        public void Create()
        {
            _adminrole = 1;
            _userrole = 2;
            _nulluser = "";
            _nullpass = "";
            _wronguser = "trung";
            _wrongpass = "123456";
            _password = "123";
            _user = "user";
            _admin = "admin";
            _oraAppUserService = new Mock<OraAppUserService>();
        }
        /// <summary>
        /// Cleanup after execute tests
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _oraAppUserService = null;
        }
        /// <summary>
        /// Login role admin success
        /// </summary>
        [TestMethod]
        public void LoginAdminSuccess()
        {
            int i = _oraAppUserService.Object.CheckRole(_admin);
            Assert.AreEqual(_adminrole, i);
        }
        //Login role user success
        [TestMethod]
        public void LoginUserSuccess()
        {
            int i = _oraAppUserService.Object.CheckRole(_user);
            Assert.AreEqual(_userrole, i);
        }
        //Login not admin role
        [TestMethod]
        public void LoginNotAdminRole()
        {
            int i = _oraAppUserService.Object.CheckRole(_user);
            Assert.AreNotEqual(_adminrole, i);
        }
        //Login not user role
        [TestMethod]
        public void LoginNotUserRole()
        {
            int i = _oraAppUserService.Object.CheckRole(_admin);
            Assert.AreNotEqual(_userrole, i);
        }
        //Login success
        [TestMethod]
        public void LoginSuccess()
        {
            APPUSER result = _oraAppUserService.Object.Check(_user, _password);
            Assert.AreEqual(_user, result.USER_NAME);
            Assert.AreEqual(_password, result.PWD);
        }
        //Login null pass and user
        [TestMethod]
        public void LoginNull()
        {
            APPUSER result = _oraAppUserService.Object.Check(_nulluser, _nullpass);
            Assert.IsNull(result);
        }
        //Login null pass
        [TestMethod]
        public void LoginPassNull()
        {
            APPUSER result = _oraAppUserService.Object.Check(_user, _nullpass);
            Assert.IsNull(result);
        }
        //Login null user
        [TestMethod]
        public void LoginUserNull()
        {
            APPUSER result = _oraAppUserService.Object.Check(_nulluser, _password);
            Assert.IsNull(result);
        }
        //login wrong username and password
        [TestMethod]
        public void LoginWrongUser()
        {
            APPUSER result = _oraAppUserService.Object.Check(_wronguser, _wrongpass);
            Assert.IsNull(result);
        }
    }
}
