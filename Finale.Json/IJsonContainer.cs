namespace Finale.Json {
    //used to add values to the container by parser
    interface IJsonContainer {
        bool IsArray { get; }
        void InternalAdd(string key, object value);
    }
}