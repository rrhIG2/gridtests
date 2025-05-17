using Newtonsoft.Json;

public class LoginData
{
    [JsonProperty("nickname")]
    public string nickname { get; set; }

    [JsonProperty("password")]
    public string password { get; set; }
}