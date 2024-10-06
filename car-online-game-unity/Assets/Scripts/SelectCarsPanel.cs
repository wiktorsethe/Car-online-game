using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectCarsPanel : MonoBehaviour
{
    [System.Serializable]
    public class Car
    {
        public string id;
        public string producer;
        public string model;

        // Constructor for initializing a Car object
        public Car(string id, string producer, string model)
        {
            this.id = id;
            this.producer = producer;
            this.model = model;
        }
    }
    
    public List<Car> Cars = new List<Car>();
    private readonly string _serverUrl = "http://54.38.52.204/getcars.php";
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private List<GameObject> listOfCarObjectsInPanel;
    [SerializeField] private SceneChanger sceneChanger;
    
    private Car _selectedCar;

    private void Start()
    {
        // Initially no car is selected
        _selectedCar = null;
        
        var auth = FindObjectOfType<CustomAuthenticator>();
        
        // Fetch the cars for the current player
        SelectUserCars(auth.playerID);
    }

    // Function to select and display cars of a user
    private void SelectUserCars(string id)
    {
        // Clear existing car objects
        DestroyChildren();
        
        StartCoroutine(GetUserCars(id));
    }

    // Destroys all car objects in the panel
    private void DestroyChildren()
    {
        if (listOfCarObjectsInPanel.Count > 0)
        {
            foreach (GameObject child in listOfCarObjectsInPanel)
            {
                Destroy(child);
            }
            // Clear the list after destruction
            listOfCarObjectsInPanel.Clear();
        }
    }

    // Coroutine to get the user's cars from the server
    IEnumerator GetUserCars(string userId)
    {
        // Prepare the form data with the user ID
        WWWForm form = new WWWForm();
        form.AddField("userid", userId);

        // Send the form to the server and wait for the response
        WWW www = new WWW(_serverUrl, form);

        yield return www;

        // Split the response into lines
        string[] lines = www.text.Split('\n');
        
        // Process each line
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                // Split the line into parts (id, producer, model)
                string[] parts = line.Split('|');
                
                // Ensure the line has 3 parts and then display the car
                if (parts.Length == 3)
                {
                    ShowCar(parts[0], parts[1], parts[2]);
                }
            }
        }
    }

    // Function to create and display a car in the UI
    private void ShowCar(string id, string producer, string model)
    {
        // Instantiate the car prefab in the "List" transform
        GameObject car = Instantiate(carPrefab, transform.Find("List"));
        
        // Set the text of the car prefab to show producer and model
        car.GetComponentInChildren<TMP_Text>().text = producer + " - " + model;
        
        // Create a new Car object and add it to the list
        Car newCar = new Car(id, producer, model);
        Cars.Add(newCar);

        // Set up the button to handle car selection
        Button button = car.GetComponentInChildren<Button>();
        if (button != null)
        {
            int index = Cars.Count - 1;
            
            button.onClick.AddListener(() => ChoiceOfCar(index));
        }

        // Add the car UI object to the list of objects in the panel
        listOfCarObjectsInPanel.Add(car);
    }

    // Function to handle car selection based on index
    private void ChoiceOfCar(int index)
    {
        // Ensure the index is valid
        if (index >= 0 && index < Cars.Count)
        {
            // Set the selected car
            _selectedCar = Cars[index];
            
            // Get the network identity and update the player's display sprite
            var ni = NetworkClient.connection.identity;
            ni.GetComponent<UserCarInfo>().SetDisplaySprite(_selectedCar.id);
            
            // Log the selected car to the console
            Debug.Log("Selected Character: " + _selectedCar.producer + " - " + _selectedCar.model);
        }
    }
    
    // Function to handle the Play button press
    public void PlayButton(string sceneName)
    {
        // Only proceed if a car is selected
        if(_selectedCar == null) return;
        
        // Change the scene using the scene changer
        sceneChanger.ChangeScene(sceneName);
        
        enabled = false;
    }
}
