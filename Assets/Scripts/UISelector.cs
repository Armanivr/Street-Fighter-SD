using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UISelector : MonoBehaviour
{
    public static UISelector Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to set focus after one frame
    public IEnumerator SetSelectedAfterFrame(GameObject obj)
    {
        yield return null; // wait one frame
        EventSystem.current.SetSelectedGameObject(obj);
    }

    // Method to wait briefly then set focus
    public IEnumerator WaitForMenuTransitionAndSelect(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f); // adjust delay as needed
        EventSystem.current.SetSelectedGameObject(obj);
    }
}