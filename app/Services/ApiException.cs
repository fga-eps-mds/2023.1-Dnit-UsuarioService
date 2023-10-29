using api;
using EnumsNET;
using Newtonsoft.Json;

namespace app.Services
{
    public class ApiException : Exception
    {
        public ErrorModel Error { get; set; }

        public ApiException(ErrorCodes error, string? message = null, object? details = null) : base(message ?? error.AsString(EnumFormat.Description))
        {
            Error = new ErrorModel
            {
                Code = error,
                Message = Message,
                CodeStr = error.ToString(),
                Details = getDetailsDictionary(details),
            };
        }

        private static Dictionary<string, string>? getDetailsDictionary(object? details)
        {
            if (details == null)
            {
                return null;
            }
            var dic = new Dictionary<string, string>();
            foreach (var descriptor in details.GetType().GetProperties())
            {
                dic[descriptor.Name] = descriptor.GetValue(details, null)?.ToString() ?? "null";
            }
            return dic;
        }
    }

    public class ErrorModel
    {
        [JsonProperty("code")]
        public string CodeStr { get; set; }

        [JsonIgnore]
        public ErrorCodes Code
        {
            get
            {
                ErrorCodes res;
                try
                {
                    res = (ErrorCodes)Enum.Parse(typeof(ErrorCodes), CodeStr);
                }
                catch
                {
                    res = ErrorCodes.Unknown;
                }
                return res;
            }
            set
            {
                CodeStr = value.ToString();
            }
        }

        public string Message { get; set; }

        public Dictionary<string, string> Details { get; set; }

        public override string ToString()
        {
            var detailsString = string.Empty;

            if (Details != null && Details.Count > 0)
            {
                detailsString = Environment.NewLine + string.Join(Environment.NewLine, Details);
            }
            return $"{CodeStr} - {Message}{detailsString}";
        }
    }
}
