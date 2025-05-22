using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSelectionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] p1Buttons;             // Player 1 character buttons
    public Button[] p2Buttons;             // Player 2 character buttons
    public GameObject p2SelectionPanel;    // Panel containing Player 2's buttons
    public GameObject p1SelectionPanel;    // Panel containing Player 1's buttons

    [Header("Highlight Colors")]
    public Color p1HighlightColor = Color.blue;
    public Color p2HighlightColor = Color.red;
    public Color defaultColor = Color.white;

    private string p1SelectedCharacter = "";
    private string p2SelectedCharacter = "";

    private bool isP1Done = false;
    private bool isP2Done = false;

    private Button currentSelectedButtonP1;
    private Button currentSelectedButtonP2;

    void Start()
    {
        // Initialize: Player 1's selection active, Player 2's disabled
        p1SelectionPanel.SetActive(true);
        p2SelectionPanel.SetActive(false);

        // Add listeners to buttons
        foreach (Button btn in p1Buttons)
        {
            btn.onClick.AddListener(() => OnP1ButtonClicked(btn));
            ResetButtonColor(btn);
        }

        foreach (Button btn in p2Buttons)
        {
            btn.onClick.AddListener(() => OnP2ButtonClicked(btn));
            ResetButtonColor(btn);
        }
    }

    void Update()
    {
        // Handle keyboard/controller navigation
        HandleNavigation();

        // Confirm selection with Enter (keyboard) or controller button
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            if (!isP1Done)
            {
                // Confirm P1 selection
                if (currentSelectedButtonP1 != null)
                {
                    OnP1ButtonClicked(currentSelectedButtonP1);
                }
            }
            else if (!isP2Done && currentSelectedButtonP2 != null)
            {
                // Confirm P2 selection
                OnP2ButtonClicked(currentSelectedButtonP2);
            }
        }
    }

    void HandleNavigation()
    {
        // For simplicity, we'll handle navigation based on current selected button
        // and arrow keys / joystick axes

        // This example uses arrow keys and WASD for keyboard
        // and D-Pad / Left stick for controllers

        // P1 Navigation
        if (!isP1Done)
        {
            Button nextButton = null;
            Button prevButton = null;

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                nextButton = GetNextButton(p1Buttons, currentSelectedButtonP1, true);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                prevButton = GetNextButton(p1Buttons, currentSelectedButtonP1, false);
            }

            if (nextButton != null)
            {
                SetCurrentSelection(nextButton);
            }
            else if (prevButton != null)
            {
                SetCurrentSelection(prevButton);
            }
        }
        // P2 Navigation
        else if (!isP2Done)
        {
            Button nextButton = null;
            Button prevButton = null;

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                nextButton = GetNextButton(p2Buttons, currentSelectedButtonP2, true);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                prevButton = GetNextButton(p2Buttons, currentSelectedButtonP2, false);
            }

            if (nextButton != null)
            {
                SetCurrentSelection(nextButton, false);
            }
            else if (prevButton != null)
            {
                SetCurrentSelection(prevButton, false);
            }
        }
    }

    Button GetNextButton(Button[] buttons, Button current, bool forward)
    {
        if (buttons.Length == 0) return null;

        int index = 0;
        if (current != null)
        {
            index = System.Array.IndexOf(buttons, current);
            if (forward)
                index = (index + 1) % buttons.Length;
            else
                index = (index - 1 + buttons.Length) % buttons.Length;
        }
        else
        {
            index = 0;
        }
        return buttons[index];
    }

    void SetCurrentSelection(Button btn, bool isP1 = true)
    {
        EventSystem.current.SetSelectedGameObject(btn.gameObject);
        if (isP1)
        {
            currentSelectedButtonP1 = btn;
        }
        else
        {
            currentSelectedButtonP2 = btn;
        }
    }

    void OnP1ButtonClicked(Button btn)
    {
        // Save selection
        p1SelectedCharacter = btn.name; // or use a specific property
        HighlightButton(btn, p1HighlightColor);
        currentSelectedButtonP1 = btn;
        isP1Done = true;

        // Disable Player 1 buttons
        foreach (Button b in p1Buttons)
        {
            b.interactable = false;
        }

        // Enable Player 2 selection
        p2SelectionPanel.SetActive(true);
        // Set default selection for P2
        if (p2Buttons.Length > 0)
        {
            SetCurrentSelection(p2Buttons[0], false);
        }
    }

    void OnP2ButtonClicked(Button btn)
    {
        // Save selection
        p2SelectedCharacter = btn.name;
        HighlightButton(btn, p2HighlightColor);
        currentSelectedButtonP2 = btn;
        isP2Done = true;

        // Finalize selections
        Debug.Log("Player 1 selected: " + p1SelectedCharacter);
        Debug.Log("Player 2 selected: " + p2SelectedCharacter);

        // Proceed to next scene or start game
        // e.g., SceneManager.LoadScene("GameScene");
    }

    void HighlightButton(Button btn, Color color)
    {
        var colors = btn.colors;
        colors.normalColor = color;
        btn.colors = colors;
    }

    void ResetButtonColor(Button btn)
    {
        var colors = btn.colors;
        colors.normalColor = defaultColor;
        btn.colors = colors;
    }
}