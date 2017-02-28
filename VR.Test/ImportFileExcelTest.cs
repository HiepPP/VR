using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VR.Service.Services;
using System.IO;
using System.Web.Configuration;
using System.Web;
using System.Net;
using VR.DAL.Model;
using System.Configuration;
using Moq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace VR.Test
{
    /// <summary>
    /// Test ImportExcel Function
    /// </summary>
    [TestClass]
    public class ImportFileExcelTest
    {
        private Mock<FileExcelService> _fileExcelService;
        private FileStream file;
        private Mock<HttpPostedFileBase> fileHttpPostedFileBaseMock;
        private FileWrapper testExisted;
        private string strVoyageCode;
        private string strVoyageCodeWrong;
        private string strFileSuccessPath;
        private string strFileContentWrong;
        private string strFileFailPath;
        private string uploadDirectory;

        /// <summary>
        /// Initialize
        /// </summary>
        [TestInitialize]
        public void Create()
        {
            strVoyageCode = "GDFN";
            strVoyageCodeWrong = "GGGG";
            strFileSuccessPath = "C:\\ThongTinXe.xlsx";
            strFileContentWrong = "C:\\ThongTinXeContentSizeWrong.xlsx";
            strFileFailPath = "C:\\ThongTinXe.docx";
            _fileExcelService = new Mock<FileExcelService>();
            fileHttpPostedFileBaseMock = new Mock<HttpPostedFileBase>();
            uploadDirectory = ConfigurationManager.AppSettings["UploadDirectory"];
            testExisted = new FileWrapper();
        }
        /// <summary>
        /// Cleanup after execute tests
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            strVoyageCode = string.Empty;
            strVoyageCodeWrong = string.Empty;
            strFileSuccessPath = string.Empty;
            strFileFailPath = string.Empty;
            _fileExcelService = null;
            file = null;
            fileHttpPostedFileBaseMock = null;
        }
        /// <summary>
        /// Import Excel Success
        /// </summary>
        [TestMethod]
        public void ImportExcel_FileSuccess()
        {
            using (file = File.Open(strFileSuccessPath, FileMode.Open))
            {
                int[] i = importFileTestFunction(file, strVoyageCode);
                Assert.AreEqual((int)ImportedFileState.Done, i[0]);
            }
        }
        /// <summary>
        /// File Excel in wrong extension
        /// </summary>
        [TestMethod]
        public void ImportExcel_WrongExtentsion()
        {
            using (file = File.Open(strFileFailPath, FileMode.Open))
            {
                int[] i = importFileTestFunction(file, strVoyageCode);
                Assert.AreEqual((int)ImportedFileState.WrongExtension, i[0]);
            }
        }
        /// <summary>
        /// File Excel not enough params
        /// </summary>
        [TestMethod]
        public void ImportExcel_NotEnoughParams()
        {
            using (file = File.Open(strFileFailPath, FileMode.Open))
            {
                file = File.Open(strFileSuccessPath, FileMode.Open);
                int[] i = importFileTestFunction(file, null);
                Assert.AreEqual((int)ImportedFileState.NotEnoughParams, i[0]);
            }
        }
        /// <summary>
        /// File Excel's content size is wrong
        /// </summary>
        [TestMethod]
        public void ImportExcel_ContentSizeWrong()
        {
            using (file = File.Open(strFileContentWrong, FileMode.Open))
            {
                int[] i = importFileTestFunction(file, strVoyageCode);
                Assert.AreEqual((int)ImportedFileState.ContentSizeWrong, i[0]);
            }
        }
        /// <summary>
        /// Voyage Code not available
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ImportExcel_VoyageNotAvail()
        {
            using (file = File.Open(strFileSuccessPath, FileMode.Open))
            {
                int[] i = importFileTestFunction(file, strVoyageCodeWrong);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ImportExcel_FileExisted()
        {
            int[] i;
            using (file = File.Open(strFileSuccessPath, FileMode.Open))
            {
                i = importFileTestFunction(file, strVoyageCode);
            }
            using (file = File.Open(strFileSuccessPath, FileMode.Open))
            {
                _fileExcelService = new Mock<FileExcelService>(testExisted);

                i = importFileTestFunction(file, strVoyageCode);
            }
            Assert.AreEqual((int)ImportedFileState.Existed, i[0]);

        }
        /// <summary>
        /// Function's used for test methods
        /// </summary>
        private int[] importFileTestFunction(FileStream file, string voyCode)
        {
            fileHttpPostedFileBaseMock = new Mock<HttpPostedFileBase>();
            //Initialize virtual HttpPostedFileBase
            fileHttpPostedFileBaseMock.Setup(f => f.ContentLength).Returns(1);
            fileHttpPostedFileBaseMock.Setup(f => f.InputStream).Returns(file);
            string[] fileName = Regex.Split(file.Name, @"[\\]");

            fileHttpPostedFileBaseMock.Setup(f => f.FileName).Returns(fileName[fileName.Length-1]);
            //Execute function
            int[] i = _fileExcelService.Object.Import(voyCode, "admin", 1, fileHttpPostedFileBaseMock.Object);
            testExisted = _fileExcelService.Object.CheckExisted();
            //file.Flush();
            //file.Close();
            return i;
        }
    }
}
