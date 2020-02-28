using NgUtil.Generics.Kvps.Impl;

namespace NgHTTP.Storage {
    public abstract class Application {

        public string Domain { get; }

        public StringStringKeyValuePairContainer LocalDatabase { get; }

        public StringStringKeyValuePairContainer LocalStorage { get; }

        public StringStringKeyValuePairContainer SessionStorage { get; }

        public StringStringKeyValuePairContainer Cache { get; }


        public Application(string domain) {
            Domain = domain;
            LocalDatabase = new StringStringKeyValuePairContainer();
            LocalStorage = new StringStringKeyValuePairContainer();
            SessionStorage = new StringStringKeyValuePairContainer();
            Cache = new StringStringKeyValuePairContainer();
        }

        public virtual Application Clear() {
            LocalDatabase.Kvps.Clear();
            LocalStorage.Kvps.Clear();
            SessionStorage.Kvps.Clear();
            Cache.Kvps.Clear();
            return this;
        }

        public override string ToString() {
            return "Application: " + Domain;
        }

    }
}
