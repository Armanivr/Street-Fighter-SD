using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject optionsMenu;

    [Header("First Selected UI Elements")]
    public GameObject playFirstSelected;
    public GameObject optionsFirstSelected;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogWarning("No PlayerInput component found on MenuManager. Add one manually in the Inspector.");
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

        // Initialize both menus to known states
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);

        // Then show the main menu
        ShowMainMenu();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (optionsMenu.activeSelf)
        {
            ShowMainMenu();
        }
    }

    public void ShowMainMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);

        Debug.Log($"Showing Main Menu. Active: {mainMenu.activeSelf}");

        // Ensure the main menu is active before selecting
        if (mainMenu.activeInHierarchy)
        {
            UISelector.Instance.SetSelectedAfterFrame(playFirstSelected);
        }
        else
        {
            Debug.LogError("Main menu is not active when trying to show it!");
        }
    }

    // Change the ShowOptionsMenu method to:
    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);

        Debug.Log($"Showing Options Menu. Active: {optionsMenu.activeSelf}");

        // Use the UISelector instance directly instead of MainMenu's coroutine
        UISelector.Instance.StartCoroutine(
            UISelector.Instance.WaitForMenuTransitionAndSelect(optionsFirstSelected)
        );
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}