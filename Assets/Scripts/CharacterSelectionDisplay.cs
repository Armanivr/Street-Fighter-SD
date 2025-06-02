using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionDisplay : MonoBehaviour
{
    public Image bigImageP1;   // assign in inspector
    public Image bigImageP2;   // assign in inspector

    public Text P1Ctext;        // assign in inspector
    public Text P2Ctext;        // assign in inspector

    public Button confirmButton; // Reference to the confirm button

    private bool isPlayer2Confirmed = false;

    void Awake()
    {
        if (P1Ctext == null)
        {
            GameObject obj = GameObject.Find("P1Ctext");
            if (obj != null)
                P1Ctext = obj.GetComponent<Text>();
            else
                Debug.LogError("Could not find 'P1Ctext' GameObject");
        }

        if (P2Ctext == null)
        {
            GameObject obj = GameObject.Find("P2Ctext");
            if (obj != null)
                P2Ctext = obj.GetComponent<Text>();
            else
                Debug.LogError("Could not find 'P2Ctext' GameObject");
        }

        if (confirmButton == null)
        {
            Debug.LogError("Confirm Button not assigned in inspector");
        }
    }

    void Start()
    {
        // Hide confirm button initially
        if (confirmButton != null)
            confirmButton.gameObject.SetActive(false);
    }

    public void OnCharacterSelected(int playerNumber, Sprite sprite, string characterName)
    {
        if (playerNumber == 1)
        {
            UpdatePlayer1(sprite, characterName);
        }
        else if (playerNumber == 2)
        {
            UpdatePlayer2(sprite, characterName);
            // Reset confirmation status
            isPlayer2Confirmed = false;
            if (confirmButton != null)
                confirmButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Invalid player number: " + playerNumber);
        }
    }

    public void UpdatePlayer1(Sprite sprite, string characterName)
    {
        if (bigImageP1 != null)
            bigImageP1.sprite = sprite;
        if (P1Ctext != null)
            P1Ctext.text = characterName;
    }

    public void UpdatePlayer2(Sprite sprite, string characterName)
    {
        if (bigImageP2 != null)
            bigImageP2.sprite = sprite;
        if (P2Ctext != null)
            P2Ctext.text = characterName;

        // Enable confirm button once Player 2 has selected
        if (confirmButton != null)
            confirmButton.gameObject.SetActive(true);

        // Reset confirmation status
        isPlayer2Confirmed = false;
    }

    // Call this from your UI Confirm Button
    public void ConfirmPlayer2Selection()
    {
        if (string.IsNullOrEmpty(P2Ctext.text))
        {
            Debug.LogWarning("Player 2 has not selected a character yet!");
            return;
        }

        isPlayer2Confirmed = true;
        Debug.Log("Player 2 has confirmed their selection.");
    }

    // Method to be called externally to start the game after confirmation
    public void OnStartGame()
    {
        if (isPlayer2Confirmed)
        {
            Debug.Log("Starting game...");
            // Call a method in MenuManager to start the game
            MenuManager.Instance.StartGame(); // Assuming singleton pattern
        }
        else
        {
            Debug.LogWarning("Player 2 must confirm selection before starting the game.");
        }
    }
}