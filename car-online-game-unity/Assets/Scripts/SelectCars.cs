using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;
public class SelectCars : MonoBehaviour
{
    [System.Serializable]
    public class Car
    {
        public string producer;
        public string model;

        public Car(string producer, string model)
        {
            this.producer = producer;
            this.model = model;
        }
    }
    
    public List<Car> Cars = new List<Car>();
    
    private string serverUrl = "http://54.38.52.204/getcars.php";
    [SerializeField] private GameObject carPrefab;
    //[SerializeField] private GameObject createCarPanel;
    [SerializeField] private List<GameObject> listOfCarObjects;
    [SerializeField] private SceneChanger _sceneChanger;
    private void Start()
    {
        var auth = FindObjectOfType<CustomAuthenticator>();
        SelectUserCars(auth.playerID);
    }

    public void SelectUserCars(string id)
    {
        DestroyChildren();
        StartCoroutine(GetUserCars(id));
    }

    private void DestroyChildren()
    {
        if (listOfCarObjects.Count > 0)
        {
            foreach (GameObject child in listOfCarObjects)
            {
                Destroy(child);
            }
            listOfCarObjects.Clear();
        }
    }
    IEnumerator GetUserCars(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userid", userId);

        WWW www = new WWW(serverUrl, form);

        yield return www;

        string[] lines = www.text.Split('\n');
        foreach (string line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                {
                    ShowCar(parts[0], parts[1]);
                }
            }
        }
    }

    private void ShowCar(string producer, string model)
    {
        GameObject car = Instantiate(carPrefab, transform.Find("List"));
        car.GetComponentInChildren<TMP_Text>().text = producer + " - " + model;
        
        Car newCar = new Car(producer, model);
        Cars.Add(newCar);

        Button button = car.GetComponentInChildren<Button>();
        if (button != null)
        {
            int index = Cars.Count - 1;
            button.onClick.AddListener(() => ChoiceOfCar(index));
        }
        
        listOfCarObjects.Add(car);
    }

    private void ChoiceOfCar(int index)
    {
        if (index >= 0 && index < Cars.Count)
        {
            Car selectedCar = Cars[index];
            Debug.Log("Selected Character: " + selectedCar.producer + " - " + selectedCar.model);
        }
    }
    
    public void PlayButton(string sceneName)
    {
        _sceneChanger.ChangeScene(sceneName);
        enabled = false;
    }
}
