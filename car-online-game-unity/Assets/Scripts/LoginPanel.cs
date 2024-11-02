using System.Collections;
using System.Net.NetworkInformation;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    // Reference to the custom network manager
    public AdditiveNetworkManager myNetworkManagerScript;
    
    // UI elements: login button, canvas, and input fields for login and password
    [SerializeField] private Button loginButton;
    [SerializeField] private GameObject canvas;
    [SerializeField] private InputField loginField;
    [SerializeField] private InputField passwordField;

    // URL of the server where login information is validated
    private string serverUrl = "http://54.38.52.204/getlogin.php";

    // Called when the object is initialized; ensures the network manager reference is set
    private void Awake()
    {
        if (myNetworkManagerScript == null)
        {
            myNetworkManagerScript = FindObjectOfType<AdditiveNetworkManager>();
        }
    }

    // Called when the script starts; adds the login button click event listener
    public void Start()
    {
        Button btn = loginButton.GetComponent<Button>();
        btn.onClick.AddListener(LoginOnClick); // Links the button click to the login action
    }

    // Function triggered on login button click
    private void LoginOnClick()
    {
        // Initiates the login coroutine with the entered username and password
        StartCoroutine(LoginDB(loginField.text, passwordField.text));
    }

    // Coroutine to handle login authentication with the server
    IEnumerator LoginDB(string username, string password)
    {
        // Create a form to send username and password to the server
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        // Send the form to the server for validation
        WWW www = new WWW(serverUrl, form);

        // Wait for the server's response
        yield return www;

        // Parse the response from the server, expected to be in the format "id|username|status"
        string[] userData = www.text.Split('|');
        if (userData.Length == 2)
        {
            // On successful login, set player ID and name in the authenticator
            var auth = FindObjectOfType<CustomAuthenticator>();
            auth.SetPlayerId(userData[0]);
            auth.SetPlayername(userData[1]);

            // Start the client network connection
            myNetworkManagerScript.StartClient(); // ZAMIEN NA START CLIENT !!!!!!!!!!!!
        }
        else
        {
            // On login failure, clear the input fields and log the server response
            loginField.text = "";
            passwordField.text = "";
            Debug.Log(www.text); // Show error response
        }
    }


}
