using System.Configuration;
using NUnit.Framework;
using trainTypeEditor;
using CommonModel;
using DCModel;

namespace 车型参数修改器测试.Dto
{
    [TestFixture()]
    public class TrainTypeDtoTests
    {
        private DCContext _context;
        [TestFixtureSetUp]
        public void Init()
        {
            _context = new DCContext(string.Format(ConfigurationManager.ConnectionStrings["DCContext"].ConnectionString, "Password=sa123"));
        }
        [Test()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void NewTrainTypeTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void DeleteTest1()
        {
            Assert.Fail();
        }
    }
}


