using System.Reflection.Emit;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ApiResponse<T>
    {
        public int Code { get; }
        public string Message { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string[]>? Errors { get; }

        public ApiResponse(int code, string messsage, T? data, Dictionary<string, string[]>? errors)
        {
            Code = code;
            Message = messsage;
            Data = data;
            Errors = errors;
        }

        public static ApiResponse<T> Success(T data = default)
        {
            return new ApiResponse<T>(0, "Success", data, null);
        }

        public static ApiResponse<T> Error(Dictionary<string, string[]> errors)
        {
            return new ApiResponse<T>(1, "Errors", default, errors);
        }

        public static ApiResponse<T> NotFound(string message = "Not found")
        {
            return new ApiResponse<T>(1, message, default, default);
        }

        public static ApiResponse<T> Conflict(string message = "Conflict")
        {
            return new ApiResponse<T>(1, message, default, default);
        }

        internal static ApiResponse<bool> Error(string v)
        {
            throw new NotImplementedException();
        }
    }
}
