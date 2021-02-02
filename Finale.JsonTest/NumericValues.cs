using Finale.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Finale.JsonTest {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class NumericValues {
        [TestMethod]
        public void Zero() {
            JsonObject jo = new JsonObject("{\"value\":000000000}");
            Assert.AreEqual(0, (int)jo["value"]);
        }

        [TestMethod]
        public void NumericNegavite() {
            JsonObject JO = new JsonObject("{\"Negative Value\":-1,\"Positive Value\":2,\"PI\":3.14159,\"Negavite Double\":-0.12345}");
            Assert.AreEqual(-1, (int)JO["Negative Value"]);
        }
        [TestMethod]
        public void NumericPositive() {
            JsonObject JO = new JsonObject("{\"Negative Value\":-1,\"Positive Value\":2,\"PI\":3.14159,\"Negavite Double\":-0.12345}");
            Assert.AreEqual(2, (int)JO["Positive Value"]);
        }
        [TestMethod]
        public void NumericDouble() {
            JsonObject JO = new JsonObject("{\"Negative Value\":-1,\"Positive Value\":2,\"PI\":3.14159,\"Negavite Double\":-0.12345}");
            Assert.AreEqual(3.14159d, (double)JO["PI"]);
        }
        [TestMethod]
        public void NumericNegaviteDouble() {
            JsonObject JO = new JsonObject("{\"Negative Value\":-1,\"Positive Value\":2,\"PI\":3.14159,\"Negavite Double\":-0.12345}");
            Assert.AreEqual(-0.12345d, (double)JO["Negavite Double"]);
        }
        [TestMethod]
        public void NumericE() {
            JsonObject JO = new JsonObject("{\"Mass of earth\":5.9736e24}");
            Assert.AreEqual(5.9736e24d, (double)JO["Mass of earth"]);
        }

        [TestMethod]
        public void Culture_Fr() {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("fr");

            JsonObject jo = new JsonObject("{\"value\":1.23}");
        }
    }
}