using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;

public class DatabaseManager : MonoBehaviour
{
    // Replace with your server URL
    private const string serverUrl = "https://ukfig2.sk/arcGis/";

    public IEnumerator Register(string nickname, string password, System.Action<string> callback)
    {
        // Debugging outputs
        Debug.Log($"Nickname: {nickname}, Password: {password}");

        // Create a concrete class for serialization
        RegisterData formData = new RegisterData
        {
            nickname = nickname,
            password = password
        };

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("❌ Nickname or password is empty!");
            callback("Nickname or password is empty!");
            yield break;
        }

        // Serialize to JSON using Newtonsoft
        string json = JsonConvert.SerializeObject(formData);
        Debug.Log($"📤 Sending JSON: {json}");

        UnityWebRequest request = UnityWebRequest.PostWwwForm(serverUrl + "register.php", string.Empty);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.uploadHandler.contentType = "application/json";
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"📥 Received Response: {request.downloadHandler.text}");
            callback(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"❌ Error: {request.error}");
            callback($"Error: {request.error}");
        }
    }

    public IEnumerator Login(string nickname, string password, System.Action<string> callback)
    {
        // Create a data object for serialization
        LoginData formData = new LoginData
        {
            nickname = nickname,
            password = password
        };

        // Serialize to JSON
        string json = JsonConvert.SerializeObject(formData);
        Debug.Log($"📤 Sending JSON: {json}");

        UnityWebRequest request = UnityWebRequest.PostWwwForm(serverUrl + "login.php", string.Empty);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.uploadHandler.contentType = "application/json";
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"📥 Received Response: {request.downloadHandler.text}");

            // Deserialize response
            var response = JsonConvert.DeserializeObject<LoginResponse>(request.downloadHandler.text);

            if (response.message == "Login successful.")
            {
                // Store in SessionData
                SessionData.UserId = response.userId;
                SessionData.Nickname = response.nickname;
                Debug.Log($"✅ Session Stored: {SessionData.UserId}, {SessionData.Nickname}");
            }

            callback(response.message);
        }
        else
        {
            Debug.LogError($"❌ Error: {request.error}");
            callback($"Error: {request.error}");
        }
    }
}
