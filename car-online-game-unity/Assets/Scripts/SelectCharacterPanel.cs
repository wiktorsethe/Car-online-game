using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class SelectCharacterPanel : MonoBehaviour
{
    [System.Serializable]
    public class Character
    {
        public string nickname;
        public string gender;

        public Character(string nickname, string gender)
        {
            this.nickname = nickname;
            this.gender = gender;
        }
    }
    
    public List<Character> Characters = new List<Character>();
    
    private string serverUrl = "http://54.38.52.204/getcharacters.php";
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject createCharacterPanel;
    [SerializeField] private List<GameObject> listOfCharacterObjects;
    [SerializeField] private SceneChanger _sceneChanger;
    private void Start()
    {
        var auth = FindObjectOfType<CustomAuthenticator>();
        SelectCharacters(auth.playerID);
    }

    public void SelectCharacters(string id)
    {
        DestroyChildren();
        StartCoroutine(GetUserCharacters(id));
    }

    private void DestroyChildren()
    {
        if (listOfCharacterObjects.Count > 0)
        {
            foreach (GameObject child in listOfCharacterObjects)
            {
                Destroy(child);
            }
            listOfCharacterObjects.Clear();
        }
    }
    IEnumerator GetUserCharacters(string userId)
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
                    ShowCharacter(parts[0], parts[1]);
                }
            }
        }
    }

    private void ShowCharacter(string nickname, string gender)
    {
        GameObject character = Instantiate(characterPrefab, transform.Find("List"));
        character.GetComponentInChildren<TMP_Text>().text = nickname + " [" + gender + "]";
        
        Character newCharacter = new Character(nickname, gender);
        Characters.Add(newCharacter);

        Button button = character.GetComponentInChildren<Button>();
        if (button != null)
        {
            int index = Characters.Count - 1;
            button.onClick.AddListener(() => ChoiceOfCharacter(index));
        }
        
        listOfCharacterObjects.Add(character);
    }

    private void ChoiceOfCharacter(int index)
    {
        if (index >= 0 && index < Characters.Count)
        {
            Character selectedCharacter = Characters[index];
            Debug.Log("Selected Character: " + selectedCharacter.nickname + " [" + selectedCharacter.gender + "]");
        }
    }
    
    public void PlayButton(string sceneName)
    {
        _sceneChanger.ChangeScene(sceneName);
        enabled = false;
    }

    /*
    public void ShowCreateCharacterPanel()
    {
        gameObject.SetActive(false);
        createCharacterPanel.SetActive(true);
    }*/
}