using Newtonsoft.Json;

public class RegisterData
{
    [JsonProperty("nickname")]
    public string nickname { get; set; }

    [JsonProperty("password")]
    public string password { get; set; }
}
