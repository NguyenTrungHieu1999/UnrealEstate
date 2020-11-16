using Newtonsoft.Json;
using System.Collections.Generic;

namespace UnrealEstate.Web.Models
{
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
