using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic; // Needed for Dictionary
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject characterSelectionMenu; // The character selection UI

    [Header("First Selected UI Elements")]
    public GameObject playFirstSelected;
    public GameObject optionsFirstSelected;
    public GameObject characterFirstSelected; // Focus for character selection

    [Header("Character Display")]
    public CharacterSelectionDisplay characterDisplay; // Drag your CharacterSelectionDisplay GameObject here

    // Map character names to sprites
    public List<CharacterSpriteMapping> characterMappings;

    private Dictionary<string, Sprite> characterDictionary;

    // List of character sprite mappings - assign in inspector
    public List<CharacterSpriteMapping> characterSprites;

    // UI Image component where the sprite will be displayed
    public Image displayImage;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogWarning("No PlayerInput component found on MenuManager. Add one manually in the Inspector.");
        }

        // Initialize dictionary
        characterDictionary = new Dictionary<string, Sprite>();
        foreach (var mapping in characterMappings)
        {
            if (!characterDictionary.ContainsKey(mapping.characterName))
            {
                characterDictionary.Add(mapping.characterName, mapping.characterSprite);
            }
        }
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Cancel"].performed += OnCancel;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Cancel"].performed -= OnCancel;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Initialize all menus to inactive
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        if (characterSelectionMenu != null)
            characterSelectionMenu.SetActive(false);

        // Show the main menu at start
        ShowMainMenu();
    }

    private void OnCancel(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Cancel from options or character selection returns to main menu
        if (optionsMenu.activeSelf)
        {
            ShowMainMenu();
        }
        else if (characterSelectionMenu != null && characterSelectionMenu.activeSelf)
        {
            ShowMainMenu();
        }
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        if (characterSelectionMenu != null)
            characterSelectionMenu.SetActive(false);

        Debug.Log("Showing Main Menu");
        UISelector.Instance.SetSelectedAfterFrame(playFirstSelected);
    }

    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        if (characterSelectionMenu != null)
            characterSelectionMenu.SetActive(false);

        Debug.Log("Showing Options Menu");
        UISelector.Instance.StartCoroutine(
            UISelector.Instance.WaitForMenuTransitionAndSelect(optionsFirstSelected)
        );
    }

    // Called when "Play" button is pressed
    public void StartCharacterSelection()
    {
        Debug.Log("StartCharacterSelection called");
        mainMenu.SetActive(false);
        if (characterSelectionMenu != null)
        {
            characterSelectionMenu.SetActive(true);
            Debug.Log("Character selection menu activated");
            UISelector.Instance.StartCoroutine(
                UISelector.Instance.WaitForMenuTransitionAndSelect(characterFirstSelected)
            );
        }
        else
        {
            Debug.LogError("Character Selection Menu not assigned!");
        }
    }

    // Called when a character is selected (via button)
    public void SelectCharacter(string characterName)
    {
        Debug.Log("Attempting to select character: " + characterName);

        // Find the mapping for the given character name
        var mapping = characterSprites.Find(c => c.characterName == characterName);

        if (mapping != null)
        {
            Debug.Log("Mapping found for character: " + characterName);

            if (mapping.characterSprite != null)
            {
                Debug.Log("Found sprite for " + characterName);
                displayImage.sprite = mapping.characterSprite;
            }
            else
            {
                Debug.LogWarning("No sprite assigned for character: " + characterName);
            }
        }
        else
        {
            Debug.LogWarning("No mapping found for character: " + characterName);
        }
    }

    // Called when "Confirm" button is pressed after selecting a character
    public void ConfirmCharacterSelection()
    {
        // Load the main game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void OnCharacterButtonClicked(Sprite characterSprite, int playerNumber)
    {
        Debug.Log($"Player {playerNumber} selected sprite: {characterSprite.name}");
        // Call the display update
        if (characterDisplay != null)
        {
            characterDisplay.OnCharacterSelected(playerNumber, characterSprite);
        }
    }

    // Alternatively, you can use this method for "Play" button to go directly to game
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class CharacterSpriteMapping
{
    public string characterName; // Name used in button (e.g., in the button's OnClick)
    public Sprite characterSprite; // Corresponding sprite
}