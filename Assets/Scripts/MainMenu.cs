using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject characterSelectionMenu;

    [Header("First Selected UI Elements")]
    public GameObject playFirstSelected;
    public GameObject optionsFirstSelected;
    public GameObject characterFirstSelected;

    [Header("Character Display")]
    public CharacterSelectionDisplay characterDisplay; // Drag your CharacterSelectionDisplay GameObject here

    [Header("Character Mapping")]
    public List<CharacterSpriteMapping> characterMappings;

    private Dictionary<string, Sprite> characterDictionary;

    [Header("UI Elements to show selected players")]
    public Image displayImage;
    public Text displayText;

    private enum Player { Player1, Player2 }
    private Player currentPlayer = Player.Player1;

    public Sprite selectedSpriteP1;
    public string selectedNameP1;
    public Sprite selectedSpriteP2;
    public string selectedNameP2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Initialize dictionary
        characterDictionary = new Dictionary<string, Sprite>();
        foreach (var mapping in characterMappings)
        {
            if (!characterDictionary.ContainsKey(mapping.characterName))
            {
                characterDictionary.Add(mapping.characterName, mapping.characterSprite);
            }
        }
        ShowMainMenu();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        if (characterSelectionMenu != null)
            characterSelectionMenu.SetActive(false);
        StartCoroutine(UISelector.Instance.SetSelectedAfterFrame(playFirstSelected));
    }

    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        if (characterSelectionMenu != null)
            characterSelectionMenu.SetActive(false);
        StartCoroutine(UISelector.Instance.WaitForMenuTransitionAndSelect(optionsFirstSelected));
    }

    public void StartCharacterSelection()
    {
        mainMenu.SetActive(false);
        if (characterSelectionMenu != null)
        {
            characterSelectionMenu.SetActive(true);
            currentPlayer = Player.Player1;
            selectedSpriteP1 = null;
            selectedNameP1 = "";
            selectedSpriteP2 = null;
            selectedNameP2 = "";
            Debug.Log("Player 1, please select your character");
            StartCoroutine(UISelector.Instance.WaitForMenuTransitionAndSelect(characterFirstSelected));
        }
    }

    public void RegisterCharacterSelection(string characterName)
    {
        if (!characterDictionary.ContainsKey(characterName))
        {
            Debug.LogWarning("Character not found in dictionary: " + characterName);
            return;
        }

        Sprite sprite = characterDictionary[characterName];

        if (currentPlayer == Player.Player1)
        {
            selectedSpriteP1 = sprite;
            selectedNameP1 = characterName;
            Debug.Log("Player 1 selected: " + characterName);
            // Update display
            characterDisplay.UpdatePlayer1(sprite, characterName);
            // Prompt Player 2
            currentPlayer = Player.Player2;
            Debug.Log("Player 2, please select your character");
            StartCoroutine(UISelector.Instance.WaitForMenuTransitionAndSelect(characterFirstSelected));
        }
        else if (currentPlayer == Player.Player2)
        {
            selectedSpriteP2 = sprite;
            selectedNameP2 = characterName;
            Debug.Log("Player 2 selected: " + characterName);
            // Update display
            characterDisplay.UpdatePlayer2(sprite, characterName);
            Debug.Log("Both players have selected their characters!");
            // You can enable a "Start Game" button here in your UI
        }
    }

    // This method is called when the player clicks the start button
    public void ConfirmSelections()
    {
        if (string.IsNullOrEmpty(selectedNameP1) || string.IsNullOrEmpty(selectedNameP2))
        {
            Debug.LogWarning("Both players must select characters before starting.");
            return;
        }
        Debug.Log("Starting game with selections:");
        Debug.Log("P1: " + selectedNameP1);
        Debug.Log("P2: " + selectedNameP2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetSelections()
    {
        selectedSpriteP1 = null;
        selectedNameP1 = "";
        selectedSpriteP2 = null;
        selectedNameP2 = "";
        currentPlayer = Player.Player1;
        // Reset display
        characterDisplay.UpdatePlayer1(null, "Player 1");
        characterDisplay.UpdatePlayer2(null, "Player 2");
    }
    public void StartGame()
    {
        // Ensure both players have selected characters
        if (string.IsNullOrEmpty(selectedNameP1) || string.IsNullOrEmpty(selectedNameP2))
        {
            Debug.LogWarning("Cannot start game: both players must select characters");
            return;
        }
        Debug.Log("Starting game scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    [System.Serializable]
    public class CharacterSpriteMapping
    {
        public string characterName;
        public Sprite characterSprite;
    }
}