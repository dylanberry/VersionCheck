using System;
using System.Runtime.Serialization;

namespace VersionCheck.Common
{
    [Serializable]
    public class ClientVersionNotSupportedException : Exception, ISerializable
    {
        public string MinimumSupportedClientVersionString { get; }
        public string ApiVersionString { get; }
        public string ClientVersionString { get; }

        public ClientVersionNotSupportedException(string minimumSupportedClientVersionString, string apiVersionString, string clientVersionString)
        {
            MinimumSupportedClientVersionString = minimumSupportedClientVersionString;
            ApiVersionString = apiVersionString;
            ClientVersionString = clientVersionString;
        }

        protected ClientVersionNotSupportedException(SerializationInfo info, StreamingContext context)
        {
            MinimumSupportedClientVersionString = info.GetString(nameof(MinimumSupportedClientVersionString));
            ApiVersionString = info.GetString(nameof(ApiVersionString));
            ClientVersionString = info.GetString(nameof(ClientVersionString));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // NOTE: $type is a hint to allow simple deserialization to the type.
            // This could also be done with a large amount of code and fussing around with MediaTypeFormatters.
            info.AddValue("$type", GetType().FullName);
            info.AddValue(nameof(MinimumSupportedClientVersionString), MinimumSupportedClientVersionString);
            info.AddValue(nameof(ApiVersionString), ApiVersionString);
            info.AddValue(nameof(ClientVersionString), ClientVersionString);
        }
    }
}