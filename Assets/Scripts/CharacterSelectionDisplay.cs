using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSelectionDisplay : MonoBehaviour
{
    // References to the big display images
    public Image P1BigImage;
    public Image P2BigImage;

    // Default buttons for focus
    public GameObject P1DefaultButton;
    public GameObject P2DefaultButton;

    // Store selected sprites for each player
    private Sprite p1SelectedSprite;
    private Sprite p2SelectedSprite;

    void Start()
    {
        // Set default focus for both players when menu opens
        if (P1DefaultButton != null)
            EventSystem.current.SetSelectedGameObject(P1DefaultButton);
        if (P2DefaultButton != null)
            EventSystem.current.SetSelectedGameObject(P2DefaultButton);
    }

    // Call this method from your button's OnClick() event
    public void OnCharacterSelected(int playerNumber, Sprite characterSprite)
    {
        if (playerNumber == 1)
        {
            p1SelectedSprite = characterSprite;
            P1BigImage.sprite = characterSprite;
        }
        else if (playerNumber == 2)
        {
            p2SelectedSprite = characterSprite;
            P2BigImage.sprite = characterSprite;
        }
    }
}