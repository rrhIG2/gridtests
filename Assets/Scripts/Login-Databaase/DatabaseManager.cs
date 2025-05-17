using UnityEngine;
using UnityEngine.Networking;
    using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{

    public int LastUserId { get; private set; }
    public string LastNickname { get; private set; }

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
                LastUserId = response.userId;
                LastNickname = response.nickname;
            }

            callback(response.message);
        }
        else
        {
            Debug.LogError($"❌ Error: {request.error}");
            callback($"Error: {request.error}");
        }
    }

    // ========================
    // 🆕 Fetch Grid Data
    // ========================
    public IEnumerator FetchGridData(List<Vector2Int> gridPositions, System.Action<List<GridData>> callback)
{
    if (gridPositions == null || gridPositions.Count == 0)
    {
        Debug.LogError("❌ No grid positions to send to the backend!");
        callback(null);
        yield break;
    }

    // Serialize the grid positions to JSON
    string json = JsonConvert.SerializeObject(gridPositions);
    Debug.Log($"📤 Sending Grid Data Request: {json}");

    // Prepare the web request
    UnityWebRequest request = new UnityWebRequest(serverUrl + "getGridData.php", "POST");
    byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
    request.downloadHandler = new DownloadHandlerBuffer();
    request.SetRequestHeader("Content-Type", "application/json");

    // Wait for response
    yield return request.SendWebRequest();

    if (request.result == UnityWebRequest.Result.Success)
    {
        Debug.Log($"📥 Received Grid Data Response: {request.downloadHandler.text}");

        if (request.downloadHandler.text.StartsWith("<"))
        {
            Debug.LogError("❌ Received HTML instead of JSON. There might be a PHP error.");
            callback(null);
            yield break;
        }

        // Deserialize the response into a list of GridData
        List<GridData> gridDataList = JsonConvert.DeserializeObject<List<GridData>>(request.downloadHandler.text);

        // Return the data through the callback
        callback(gridDataList);
    }
    else
    {
        Debug.LogError($"❌ Error: {request.error}");
        callback(null);
    }
}

}
