using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public Sprite characterSprite; // assign in inspector
    public string characterName;   // assign in inspector

    public void OnClick()
    {
        // Find the MenuManager and register selection
        var menuManager = FindObjectOfType<MenuManager>();
        if (menuManager != null)
        {
            menuManager.RegisterCharacterSelection(characterName);
        }
        else
        {
            Debug.LogWarning("MenuManager not found");
        }
    }
}