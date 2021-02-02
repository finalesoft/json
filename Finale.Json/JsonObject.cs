using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Finale.Json {
    /// <summary>
    /// JsonObject class
    /// </summary>
    public class JsonObject : IJsonContainer, IDictionary<string, object> {
        Dictionary<string, object> entries;


        #region Constructor
        /// <summary>
        /// Creates an empty JsonObject
        /// </summary>
        public JsonObject() {
            entries = new Dictionary<string, object>();
        }

        /// <summary>
        /// Create a new JsonObject from a string
        /// </summary>
        /// <param name="jsonString">JSON string that represents an object</param>
        /// <exception cref="FormatException">JsonString represents JsonArray instead of JsonObject</exception>
        public JsonObject(string jsonString) {
            JsonObject jo = Parser.Parse(jsonString) as JsonObject;
            if (jo == null) throw new FormatException("JsonString represents JsonArray instead of JsonObject");
            this.entries = jo.entries;
        }

        /// <summary>
        /// Constructs a JSON object from a model object
        /// </summary>
        /// <param name="model">Annoymous type containing initial values</param>
        public JsonObject(object model) {
            var x = ModelConverter.ToJson(model);
            if (!(x is JsonObject)) throw new ArgumentException(string.Format("Model is converted to {0} instead of JsonObject", x.GetType()));

            this.entries = (x as JsonObject).entries;
        }
        #endregion

        #region IJsonContainer
        void IJsonContainer.InternalAdd(string key, object value) {
            entries.Add(key, value);
        }
        bool IJsonContainer.IsArray { get { return false; } }
        #endregion

        #region Indexer
        /// <summary>
        /// Gets a property of the current JSON Object by key
        /// </summary>
        /// <param name="key">Key of property</param>
        /// <returns>Value of property.</returns>
        public object this[string key] {
            get {
                return entries[key];
            }
            set {
                Helper.AssertValidType(value);
                entries[key] = value;
            }
        }
        #endregion

        #region Interface
        /// <summary>
        /// The number of key/value pairs contained in the JsonObject
        /// </summary>
        public int Count { get { return entries.Count; } }
        /// <summary>
        /// Whether the JsonObject is read-only. This value is always true.
        /// </summary>
        public bool IsReadOnly { get { return false; } }
        /// <summary>
        /// All the keys in the JsonObject
        /// </summary>
        public ICollection<string> Keys { get { return entries.Keys; } }
        /// <summary>
        /// All the values in the JsonObject
        /// </summary>
        public ICollection<object> Values { get { return entries.Values; } }
        /// <summary>
        /// Adds the specified key and value to the JsonObject.
        /// </summary>
        /// <param name="item"></param>
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) {
            Helper.AssertValidType(item.Value);
            Add(item.Key, item.Value);
        }
        /// <summary>
        /// Adds the specified key and value to the JsonObject.
        /// </summary>
        /// <param name="key">Key of entry</param>
        /// <param name="value">Value of entry</param>
        public void Add(string key, object value) {
            Helper.AssertValidType(value);
            entries.Add(key, value);
        }
        /// <summary>
        /// Removes all keys and values from the JsonObject.
        /// </summary>
        public void Clear() { entries.Clear(); }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) { throw new NotImplementedException(); }
        /// <summary>
        /// Removes the item with the specified key from the JsonObject.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key) { return entries.Remove(key); }
        /// <summary>
        /// Copy all the entries to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
            int i = 0;
            foreach (KeyValuePair<string, object> KVP in entries) {
                array[arrayIndex + (i++)] = KVP;
            }
        }
        /// <summary>
        /// Determines whether the JsonObject contains the specified key/value pair.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item) {
            return entries.ContainsKey(item.Key) && entries[item.Key].Equals(item.Value);
        }
        /// <summary>
        /// Determines whether the JsonObject contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key) {
            return entries.ContainsKey(key);
        }
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value) {
            return entries.TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() {
            return entries.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return entries.GetEnumerator();
        }
        #endregion

        #region ToString
        /// <summary>
        /// Returns the shortest string representation of the current JsonObject
        /// </summary>
        /// <returns>A string</returns>
        public override string ToString() {
            return ToString(false);
        }

        /// <summary>
        /// Returns the string representation of the current JsonObject
        /// </summary>
        /// <param name="beautify">Whether the string is formatted for easy reading</param>
        /// <returns>A string representation of the current JsonObject</returns>
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
        /// Populates a model object with data in the JSON object
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <returns>Model instance</returns>
        public T ToModel<T>() where T : new() {
            return (T)ModelConverter.ToModel(this, typeof(T));
        }
    }
}