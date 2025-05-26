using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    private int _lastUserId;
    private string _lastNickname;

    // Server URL (private constant)
    private const string _serverUrl = "https://ukfig2.sk/arcGis/";

    public int GetLastUserId()
    {
        return _lastUserId;
    }

    public void SetLastUserId(int value)
    {
        _lastUserId = value;
    }

    public string GetLastNickname()
    {
        return _lastNickname;
    }

    public void SetLastNickname(string value)
    {
        _lastNickname = value;
    }

    public IEnumerator Register(string nickname, string password, System.Action<string> callback)
    {
        Debug.Log($"Nickname: {nickname}, Password: {password}");

        var formData = new RegisterData
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

        string json = JsonConvert.SerializeObject(formData);
        Debug.Log($"📤 Sending JSON: {json}");

        UnityWebRequest request = UnityWebRequest.PostWwwForm(_serverUrl + "register.php", string.Empty);
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
        var formData = new LoginData
        {
            nickname = nickname,
            password = password
        };

        string json = JsonConvert.SerializeObject(formData);
        Debug.Log($"📤 Sending JSON: {json}");

        UnityWebRequest request = UnityWebRequest.PostWwwForm(_serverUrl + "login.php", string.Empty);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.uploadHandler.contentType = "application/json";
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"📥 Received Response: {request.downloadHandler.text}");
            var response = JsonConvert.DeserializeObject<LoginResponse>(request.downloadHandler.text);

            if (response.message == "Login successful.")
            {
                _lastUserId = response.userId;
                _lastNickname = response.nickname;
            }

            callback(response.message);
        }
        else
        {
            Debug.LogError($"❌ Error: {request.error}");
            callback($"Error: {request.error}");
        }
    }

    public IEnumerator FetchGridData(List<Vector2Int> gridPositions, System.Action<List<GridData>> callback)
    {
        if (gridPositions == null || gridPositions.Count == 0)
        {
            Debug.LogError("❌ No grid positions to send to the backend!");
            callback(null);
            yield break;
        }

        string json = JsonConvert.SerializeObject(gridPositions);
        Debug.Log($"📤 Sending Grid Data Request: {json}");

        UnityWebRequest request = new UnityWebRequest(_serverUrl + "getGridData.php", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

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

            List<GridData> gridDataList = JsonConvert.DeserializeObject<List<GridData>>(request.downloadHandler.text);
            callback(gridDataList);
        }
        else
        {
            Debug.LogError($"❌ Error: {request.error}");
            callback(null);
        }
    }

    public IEnumerator SendStartProductionRequest(int gridX, int gridY, int playerId, MaterialType material, double miningRate)
    {
        string fullUrl = $"{_serverUrl}startProduction.php";

        var requestData = new
        {
            grid_x = gridX,
            grid_y = gridY,
            player_id = playerId,
            material = material.ToString(),
            mining_rate = miningRate
        };

        string json = JsonConvert.SerializeObject(requestData);
        Debug.Log($"📤 Sending Start Production Request: {json}");

        UnityWebRequest request = new UnityWebRequest(fullUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"✅ Production started successfully: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"❌ Failed to start production: {request.error}");
            Debug.LogError($"🔎 Response: {request.downloadHandler.text}");
        }
    }

    public IEnumerator UpdateMaterialOnBackend(int gridX, int gridY, MaterialType material, double minedAmount)
    {
        var requestData = new
        {
            grid_x = gridX,
            grid_y = gridY,
            material = material.ToString().ToLower(),
            mined_amount = minedAmount
        };

        string json = JsonConvert.SerializeObject(requestData);

        UnityWebRequest request = new UnityWebRequest(_serverUrl + "updateMaterial.php", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"✅ Successfully updated material: {material} for Grid ({gridX}, {gridY}) - {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"❌ Failed to update material: {request.error}");
        }
    }
}
