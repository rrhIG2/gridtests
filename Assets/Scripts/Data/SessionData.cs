using Newtonsoft.Json;
using UnityEngine;

public static class SessionData
{
    public static int UserId { get; set; }
    public static string Nickname { get; set; }
    public static string Token { get; set; }  // Optional: if you use tokens for authentication

    /// <summary>
    /// Clears all session data.
    /// </summary>
    public static void ClearSession()
    {
        UserId = -1;
        Nickname = string.Empty;
        Token = string.Empty;
    }

    /// <summary>
    /// Converts the session data to a JSON string.
    /// </summary>
    public static string ToJson()
    {
        return JsonConvert.SerializeObject(new
        {
            userId = UserId,
            nickname = Nickname,
            token = Token
        });
    }

    /// <summary>
    /// Loads session data from a JSON string.
    /// </summary>
    public static void FromJson(string jsonData)
    {
        var sessionObject = JsonConvert.DeserializeObject<SessionObject>(jsonData);

        if (sessionObject != null)
        {
            UserId = sessionObject.userId;
            Nickname = sessionObject.nickname;
            Token = sessionObject.token;
        }
    }

    // Internal class for JSON deserialization
    private class SessionObject
    {
        public int userId;
        public string nickname;
        public string token;
    }
}
