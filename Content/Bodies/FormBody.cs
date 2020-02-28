using NgHTTP.Streams;
using NgHTTP.Util;
using NgUtil.Generics.Kvps.Impl;
using System;
using System.Text;
using System.Web;

namespace NgHTTP.Content.Bodies {
    public sealed class FormBody : ContentBody {

        private readonly StringStringKeyValuePairContainer kvpContainer;


        public FormBody() : base(EncType.UrlEncoded) {
            kvpContainer = new StringStringKeyValuePairContainer();
        }

        public FormBody Add(string key, object value, bool noReplace = false) {
            if (!noReplace) {
                StringStringKeyValuePair exists = kvpContainer.Get(key);

                if (exists != null) {
                    kvpContainer.Remove(exists);
                }
            }
            string stringVal = value is string ? Encoding.UTF8.GetString(Encoding.Default.GetBytes((string)value)) : value.ToString();
            kvpContainer.Put(new StringStringKeyValuePair(key, stringVal));
            return this;
        }

        public override StringStringKeyValuePairContainer GetContent<StringStringKeyValuePairContainer>() {
            return (StringStringKeyValuePairContainer)Convert.ChangeType(kvpContainer, typeof(StringStringKeyValuePairContainer));
        }

        protected override void Build(IHttpStreamable httpStream, bool prepare) {
            int index = 0;

            foreach (StringStringKeyValuePair kvp in kvpContainer.Kvps) {
                if (!kvp.Value.Equals(string.Empty)) {
                    string key = UrlEncoded ? HttpUtility.UrlEncode(kvp.Key) : kvp.Key;
                    string value = UrlEncoded ? HttpUtility.UrlEncode(kvp.Value) : kvp.Value;
                    httpStream.AppendString(key + "=" + value);
                }
                if (++index < kvpContainer.Kvps.Count) {
                    httpStream.AppendString("&");
                }
            }
        }
    }
}
