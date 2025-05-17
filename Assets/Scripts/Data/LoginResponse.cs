using Newtonsoft.Json;

public class LoginResponse
{
    [JsonProperty("message")]
    public string message { get; set; }

    [JsonProperty("userId")]
    public int userId { get; set; }

    [JsonProperty("nickname")]
    public string nickname { get; set; }
}