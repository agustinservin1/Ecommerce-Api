using Newtonsoft.Json;

namespace Application.Models.PaymentModels
{
    public class InfoPaymentNotification
    {
        public InfoPaymentNotification()
        {
            Action = "payment.updated";
            ApiVersion = "v1";
            LiveMode = false;
            Type = "payment";
        }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("live_mode")]
        public bool LiveMode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date_created")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }

    public class Data
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
