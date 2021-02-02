using Finale.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Finale.JsonTest {
    [TestClass]
    public class DataTest {
        public DataTest() { }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [DeploymentItem(@"Data\JoParse_Bad.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "JoParse_Bad.xml", "JsonParseCase", DataAccessMethod.Random), TestMethod]
        public void JsonObject_Bad_Parse() {
            try {
                var jo = new JsonObject(TestContext.DataRow["Input"].ToString());
                Assert.Fail();
            } catch (AssertFailedException) {
                throw;
            } catch (FormatException) {
            }
        }

        [DeploymentItem(@"Data\JoParse.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "JoParse.xml", "JsonParseCase", DataAccessMethod.Random), TestMethod]
        public void JsonObject_Parse() {
            var jo = new JsonObject(TestContext.DataRow["Input"].ToString());
        }

        [DeploymentItem(@"Data\JoMinify.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "JoMinify.xml", "JsonTestCase", DataAccessMethod.Random), TestMethod]
        public void JsonObject_Minify() {
            var jo = new JsonObject(TestContext.DataRow["Input"].ToString());
            Assert.AreEqual(TestContext.DataRow["Answer"].ToString(), jo.ToString());
        }

        [DeploymentItem(@"Data\JoParse.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "JoParse.xml", "JsonParseCase", DataAccessMethod.Random), TestMethod]
        public void JsonObject_ToString() {
            JsonObject jo1 = new JsonObject(TestContext.DataRow["Input"].ToString());
            JsonObject jo2 = new JsonObject(jo1.ToString());
            Assert.AreEqual(jo1.ToString(), jo2.ToString());
        }

        [DeploymentItem(@"Data\JaParse.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "JaParse.xml", "JAParseCase", DataAccessMethod.Random), TestMethod]
        public void JsonArray_ToString() {
            JsonArray ja1 = new JsonArray(TestContext.DataRow["Input"].ToString());
            JsonArray ja2 = new JsonArray(ja1.ToString());
            Assert.AreEqual(ja1.ToString(), ja2.ToString());
        }
    }
}