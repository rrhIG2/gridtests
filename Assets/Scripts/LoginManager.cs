using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private DatabaseManager _databaseManager;

    [Header("Canvases")]
    [SerializeField] private Canvas _loginCanvas;
    [SerializeField] private Canvas _registerCanvas;

    [Header("Login UI Elements")]
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private TextMeshProUGUI _loginMessageText;

    [Header("Register UI Elements")]
    [SerializeField] private TMP_InputField _nicknameRegisterInput;
    [SerializeField] private TMP_InputField _passwordRegisterInput;
    [SerializeField] private TMP_InputField _passwordAgainRegisterInput;
    [SerializeField] private TextMeshProUGUI _registerMessageText;

    /// <summary>
    /// Awake: Check if all necessary fields are assigned
    /// </summary>
    private void Awake()
    {
        if (_loginCanvas == null) Debug.LogError("❌ Login Canvas is not assigned.");
        if (_registerCanvas == null) Debug.LogError("❌ Register Canvas is not assigned.");

        if (_nicknameInput == null) Debug.LogError("❌ Nickname Input is not assigned.");
        if (_passwordInput == null) Debug.LogError("❌ Password Input is not assigned.");
        if (_loginMessageText == null) Debug.LogError("❌ Login Message Text is not assigned.");

        if (_nicknameRegisterInput == null) Debug.LogError("❌ Nickname Register Input is not assigned.");
        if (_passwordRegisterInput == null) Debug.LogError("❌ Password Register Input is not assigned.");
        if (_passwordAgainRegisterInput == null) Debug.LogError("❌ Password Again Register Input is not assigned.");
        if (_registerMessageText == null) Debug.LogError("❌ Register Message Text is not assigned.");
    }

    /// <summary>
    /// Start: Enable Login Canvas and disable Register Canvas
    /// </summary>
    private void Start()
    {
        _loginCanvas.enabled = true;
        _registerCanvas.enabled = false;
        Debug.Log("✅ Login Canvas is active, Register Canvas is hidden.");
    }

    /// <summary>
    /// Called when the Register button is clicked
    /// </summary>
    public void OnRegisterClick()
    {
        string nickname = _nicknameRegisterInput.text;
        string password = _passwordRegisterInput.text;
        string passwordAgain = _passwordAgainRegisterInput.text;

        if (password != passwordAgain)
        {
            _registerMessageText.text = "Passwords do not match.";
            return;
        }

        StartCoroutine(_databaseManager.Register(nickname, password, (response) =>
        {
            _registerMessageText.text = response;
        }));
    }

    /// <summary>
    /// Called when the Login button is clicked
    /// </summary>
    public void OnLoginClick()
    {
        string nickname = _nicknameInput.text;
        string password = _passwordInput.text;

        StartCoroutine(_databaseManager.Login(nickname, password, (response) =>
        {
            _loginMessageText.text = response;

            if (response == "Login successful.")
            {
                PlayerPrefs.SetInt("UserId", _databaseManager.GetLastUserId());
                PlayerPrefs.SetString("Nickname", _databaseManager.GetLastNickname());
                PlayerPrefs.Save(); // optional but ensures data is written immediately

                Debug.Log($"✅ PlayerPrefs Stored: {PlayerPrefs.GetInt("UserId")}, {PlayerPrefs.GetString("Nickname")}");
                SceneManager.LoadScene("Tier_1");
            }
        }));
    }

    /// <summary>
    /// Switch to Register Canvas
    /// </summary>
    public void OnRegisterCanvasClick()
    {
        _loginCanvas.enabled = false;
        _registerCanvas.enabled = true;
        Debug.Log("➡️ Switched to Register Canvas.");
    }

    /// <summary>
    /// Switch back to Login Canvas
    /// </summary>
    public void OnLoginCanvasClick()
    {
        _loginCanvas.enabled = true;
        _registerCanvas.enabled = false;
        Debug.Log("⬅️ Switched to Login Canvas.");
    }
}
