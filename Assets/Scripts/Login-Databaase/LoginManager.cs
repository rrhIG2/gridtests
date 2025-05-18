using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private DatabaseManager databaseManager;

    [Header("Canvases")]
    [SerializeField] private Canvas loginCanvas;
    [SerializeField] private Canvas registerCanvas;

    [Header("Login UI Elements")]
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI loginMessageText;

    [Header("Register UI Elements")]
    [SerializeField] private TMP_InputField nicknameRegisterInput;
    [SerializeField] private TMP_InputField passwordRegisterInput;
    [SerializeField] private TMP_InputField passwordAgainRegisterInput;
    [SerializeField] private TextMeshProUGUI registerMessageText;

    /// <summary>
    /// Awake: Check if all necessary fields are assigned
    /// </summary>
    private void Awake()
    {
        // Canvas Checks
        if (loginCanvas == null) Debug.LogError("❌ Login Canvas is not assigned.");
        if (registerCanvas == null) Debug.LogError("❌ Register Canvas is not assigned.");

        // Login UI Checks
        if (nicknameInput == null) Debug.LogError("❌ Nickname Input is not assigned.");
        if (passwordInput == null) Debug.LogError("❌ Password Input is not assigned.");
        if (loginMessageText == null) Debug.LogError("❌ Login Message Text is not assigned.");

        // Register UI Checks
        if (nicknameRegisterInput == null) Debug.LogError("❌ Nickname Register Input is not assigned.");
        if (passwordRegisterInput == null) Debug.LogError("❌ Password Register Input is not assigned.");
        if (passwordAgainRegisterInput == null) Debug.LogError("❌ Password Again Register Input is not assigned.");
        if (registerMessageText == null) Debug.LogError("❌ Register Message Text is not assigned.");
    }

    /// <summary>
    /// Start: Enable Login Canvas and disable Register Canvas
    /// </summary>
    private void Start()
    {
        loginCanvas.enabled = true;
        registerCanvas.enabled = false;
        Debug.Log("✅ Login Canvas is active, Register Canvas is hidden.");
    }

    /// <summary>
    /// Called when the Register button is clicked
    /// </summary>
    public void OnRegisterClick()
    {
        string nickname = nicknameRegisterInput.text;
        string password = passwordRegisterInput.text;
        string passwordAgain = passwordAgainRegisterInput.text;

        if (password != passwordAgain)
        {
            registerMessageText.text = "Passwords do not match.";
            return;
        }

        StartCoroutine(databaseManager.Register(nickname, password, (response) =>
        {
            registerMessageText.text = response;
        }));
    }

    /// <summary>
    /// Called when the Login button is clicked
    /// </summary>
    public void OnLoginClick()
    {
        string nickname = nicknameInput.text;
        string password = passwordInput.text;

        StartCoroutine(databaseManager.Login(nickname, password, (response) =>
        {
            loginMessageText.text = response;

            // Only update session if login is successful
            if (response == "Login successful.")
            {
                // Store in SessionData
                SessionData.UserId = databaseManager.LastUserId;
                SessionData.Nickname = databaseManager.LastNickname;

                Debug.Log($"✅ Session Stored: {SessionData.UserId}, {SessionData.Nickname}");

                // Optional: Move to next scene
                // SceneManager.LoadScene("ProfileScene");

                Debug.Log("Sesion data " + SessionData.UserId + ", " + SessionData.Nickname);
                SceneManager.LoadScene("Tier_1");
            }
        }));
    }


    /// <summary>
    /// Switch to Register Canvas
    /// </summary>
    public void OnRegisterCanvasClick()
    {
        loginCanvas.enabled = false;
        registerCanvas.enabled = true;
        Debug.Log("➡️ Switched to Register Canvas.");
    }

    /// <summary>
    /// Switch back to Login Canvas
    /// </summary>
    public void OnLoginCanvasClick()
    {
        loginCanvas.enabled = true;
        registerCanvas.enabled = false;
        Debug.Log("⬅️ Switched to Login Canvas.");
    }
}
