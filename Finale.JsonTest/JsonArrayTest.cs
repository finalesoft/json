using Finale.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Finale.JsonTest {
    [TestClass]
    public class JsonArrayTest {
        [TestMethod]
        public void AnnoymousTypeCtor() {
            object[] planets = new object[] { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" };
            JsonArray ja = new JsonArray(planets);

            Assert.AreEqual(ja.Count, planets.Length);
            for (int i = 0; i < ja.Count; i++) Assert.AreEqual(ja[i], planets[i]);
        }


        [TestMethod]
        public void JsonArray_Add() {
            JsonArray ja = new JsonArray();
            ja.Add(12345);

            Assert.AreEqual("[12345]", ja.ToString());
        }

        [TestMethod]
        public void JsonArray_Count() {
            JsonArray ja = new JsonArray();
            for (int i = 0; i < 1000; i++) ja.Add(i);
            Assert.AreEqual(1000, ja.Count);

            ja.Clear();
            Assert.AreEqual(0, ja.Count);
        }

        [TestMethod]
        public void JsonArray_Remove() {
            JsonArray ja = new JsonArray();
            for (int i = 0; i < 1000; i++) ja.Add(i);

            Assert.IsTrue(ja.Contains(123));
            Assert.AreEqual(123, ja.IndexOf(123));

            ja.Remove(123);
            Assert.IsFalse(ja.Contains(123));

            ja.RemoveAt(0);
            Assert.AreEqual(1, ja[0]);
        }

        [TestMethod]
        public void JsonArray_ForEach() {
            JsonArray ja = new JsonArray();
            int answer = 0;
            for (int i = 0; i < 1000; i++) {
                ja.Add(i);
                answer += i;
            }

            int result = 0;
            foreach (int i in ja) result += i;
            Assert.AreEqual(answer, result);
        }

        [TestMethod]
        public void JsonArray_InvalidAdd() {
            JsonArray ja = new JsonArray();
            try {
                ja.Add(DateTime.Now);
                Assert.Fail();
            } catch (Exception) { }
        }

        [TestMethod]
        public void JsonArray_Linq() {
            JsonArray ja = new JsonArray();
            for (int i = 0; i < 1000; i++) {
                JsonObject jo = new JsonObject();
                jo["id"] = i;
                jo["value"] = i * 3;
                ja.Add(jo);
            }

            JsonObject target = (from JsonObject o in ja
                                 where o["id"].Equals(3)
                                 select o).Single();

            Assert.AreEqual(3, target["id"]);
            Assert.AreEqual(9, target["value"]);

            int count = (from JsonObject o in ja
                         where (int)o["id"] % 2 == 0
                         select o).Count();

            Assert.AreEqual(500, count);
        }
    }
}
