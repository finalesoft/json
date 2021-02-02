﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Finale.Json {
    /// <summary>
    /// JsonArray class
    /// </summary>
    public class JsonArray : IJsonContainer, IList<object> {
        List<object> objects;

        #region Constructor
        /// <summary>
        /// Creates an empty JsonArray
        /// </summary>
        public JsonArray() {
            objects = new List<object>();
        }

        /// <summary>
        /// Create a new JsonArray
        /// </summary>
        /// <param name="jsonString">JSON string that represents an array</param>
        /// <exception cref="FormatException">JsonString represents JsonObject instead of JsonArray</exception>
        public JsonArray(string jsonString) {
            JsonArray ja = Parser.Parse(jsonString) as JsonArray;
            if (ja == null) throw new FormatException("JsonString represents JsonObject instead of JsonArray");
            this.objects = ja.objects;
        }

        /// <summary>
        /// Creates a JsonArray with initial values
        /// </summary>
        /// <param name="values">Array of initial values</param>
        public JsonArray(IList<object> values)
            : this() {
            foreach (var item in values) this.Add(item);
        }

        /// <summary>
        /// Constructs a JSON array from an array or IEnumerable
        /// </summary>
        /// <param name="model"></param>
        public JsonArray(IEnumerable model) {
            var x = ModelConverter.ToJson(model);
            if (!(x is JsonArray)) throw new ArgumentException(string.Format("Model is converted to {0} instead of JsonArray", x.GetType()));

            this.objects = (x as JsonArray).objects;
        }
        #endregion


        #region IJsonContainer
        void IJsonContainer.InternalAdd(string key, object value) {
            objects.Add(value);
        }
        bool IJsonContainer.IsArray { get { return true; } }
        #endregion


        #region Indexer
        /// <summary>
        /// Gets the object located at the specified index in the JsonArray
        /// </summary>
        /// <param name="index">Index of object</param>
        /// <returns></returns>
        public object this[int index] {
            get {
                return objects[index];
            }
            set {
                Helper.AssertValidType(value);
                objects[index] = value;
            }
        }

        #endregion


        #region Interface
        /// <summary>
        /// The number of objects contained in this JsonArray
        /// </summary>
        public int Count { get { return objects.Count; } }

        IEnumerator IEnumerable.GetEnumerator() {
            return objects.GetEnumerator();
        }
        IEnumerator<object> IEnumerable<object>.GetEnumerator() {
            return objects.GetEnumerator();
        }

        void ICollection<object>.CopyTo(object[] array, int arrayIndex) {
            objects.CopyTo(array, arrayIndex);
        }


        bool ICollection<object>.IsReadOnly {
            get { return false; }
        }



        /// <summary>
        /// Adds an item to the JsonArray
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <returns>Index of the added item</returns>
        public void Add(object item) {
            Helper.AssertValidType(item);
            objects.Add(item);
        }

        /// <summary>
        /// Removes all items in the JsonArray
        /// </summary>
        public void Clear() {
            objects.Clear();
        }

        /// <summary>
        /// Determines whether the JsonArray contains a specific value
        /// </summary>
        /// <param name="item">Value to be checked</param>
        /// <returns>True if the specified value is found in the JsonArray, otherwise False</returns>
        public bool Contains(object item) {
            return objects.Contains(item);
        }

        /// <summary>
        /// Determines the index of the first occurrence of a specific value
        /// </summary>
        /// <param name="item">Value to be checked</param>
        /// <returns>Index of the first occurrence of the specified value, -1 if the value is not found</returns>
        public int IndexOf(object item) {
            return objects.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the JsonArray at the specified index
        /// </summary>
        /// <param name="index">Index of item to be inserted</param>
        /// <param name="item">Value of item to be inserted</param>
        public void Insert(int index, object item) {
            Helper.AssertValidType(item);
            objects.Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specified value from the JsonArray
        /// </summary>
        /// <param name="item">Value to be removed</param>
        public bool Remove(object item) {
            return objects.Remove(item);
        }

        /// <summary>
        /// Removes the item at the specified index
        /// </summary>
        /// <param name="index">Index of item to be removed</param>
        public void RemoveAt(int index) {
            objects.RemoveAt(index);
        }

        #endregion


        #region ToString

        /// <summary>
        /// Returns the shortest string representation of the current JsonArray
        /// </summary>
        /// <returns>A string representation of the current JsonArray</returns>
        public override string ToString() {
            return ToString(false);
        }

        /// <summary>
        /// Returns the string representation of the current JsonArray
        /// </summary>
        /// <param name="beautify">Whether the string is formatted with new lines and indentations for easier reading.</param>
        /// <returns>A string representation of the current JsonArray</returns>
        public string ToString(bool beautify) {
            CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            StringBuilder sb = new StringBuilder();
            Stringifier.Stringify(this, sb, beautify);

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            return sb.ToString();
        }
        #endregion

        /// <summary>
        /// Populates an array or list with data in the JSON array
        /// </summary>
        /// <typeparam name="T">Type of Model</typeparam>
        /// <returns>Model instance</returns>
        public List<T> ToList<T>() {
            return (List<T>)ModelConverter.ToModel(this, typeof(List<T>));
        }
    }
}