
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    internal class JsonIgnoreConditionAttribute : Attribute
    {
        private JsonIgnoreCondition whenWritingNull;

        public JsonIgnoreConditionAttribute(JsonIgnoreCondition whenWritingNull)
        {
            this.whenWritingNull = whenWritingNull;
        }
    }
}