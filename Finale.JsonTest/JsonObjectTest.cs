using Finale.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finale.JsonTest {
    [TestClass]
    public class JsonObjectTest {
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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize() {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("fr");
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup() {
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("fr");
            Assert.AreEqual(culture, System.Threading.Thread.CurrentThread.CurrentCulture);
        }
        //
        #endregion



        [TestMethod]
        public void Ctor() {
            JsonObject target = new JsonObject();
            Assert.AreEqual(0, target.Count);
            Assert.AreEqual("{}", target.ToString());
        }

        [TestMethod]
        public void AnnoymousTypeCtor() {
            JsonObject jo = new JsonObject(new {
                Id = 123,
                Name = "Hello World",
                Photos = new JsonArray(new object[] { 1, 2, 3, 4, 5 })
            });

            Assert.AreEqual(123, jo["Id"]);
            Assert.AreEqual("Hello World", jo["Name"]);

            JsonArray photos = jo["Photos"] as JsonArray;
            for (int i = 0; i < 5; i++) Assert.AreEqual(i + 1, photos[i]);
        }



        [TestMethod]
        public void JsonObject_Add_Clear_Keys_Values() {
            var jo = new JsonObject();
            for (int i = 0; i < 100; i++) {
                string key = "key" + i;
                string value = "value" + i;
                jo.Add(key, value);
                Assert.AreEqual(value, jo[key]);
            }

            var keys = jo.Keys.ToArray();
            Assert.AreEqual(100, keys.Length);

            var values = jo.Values.ToArray();
            Assert.AreEqual(100, values.Length);

            jo.Clear();
            Assert.AreEqual(0, jo.Count);
        }



        [TestMethod]
        public void JsonObject_Contains_ContainsKey_Index() {
            var jo = new JsonObject();
            KeyValuePair<string, object> item = new KeyValuePair<string, object>("key", "value");

            Assert.AreEqual(false, jo.Contains(item));
            Assert.AreEqual(false, jo.ContainsKey(item.Key));

            jo.Add(item.Key, item.Value);
            Assert.AreEqual(true, jo.Contains(item));
            Assert.AreEqual(true, jo.ContainsKey(item.Key));
            Assert.AreEqual(item.Value, jo[item.Key]);

            jo.Remove(item.Key);
            Assert.AreEqual(false, jo.Contains(item));
            Assert.AreEqual(false, jo.ContainsKey(item.Key));
        }



        [TestMethod]
        public void JsonObject_ParseArray() {
            try {
                JsonObject jo = new JsonObject("[0,1,2,3,4,5]");
                Assert.Fail();
            } catch (FormatException) { }
        }

        [TestMethod]
        public void JsonArray_ParseObject() {
            try {
                JsonArray ja = new JsonArray("{}");
                Assert.Fail();
            } catch (FormatException) { }
        }
    }
}