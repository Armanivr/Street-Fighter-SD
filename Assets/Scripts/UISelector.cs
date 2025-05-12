using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class UISelector : MonoBehaviour
{
    public static UISelector Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want it to persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelected(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Tried to select a null GameObject");
            return;
        }

        StartCoroutine(DelayedSelect(obj));
    }

    public void SetSelectedAfterFrame(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("Tried to select a null GameObject");
            return;
        }

        StartCoroutine(SelectAfterEndOfFrame(obj));
    }

    public IEnumerator WaitForMenuTransitionAndSelect(GameObject selectedObject)
    {
        if (selectedObject == null)
        {
            Debug.LogWarning("Tried to select a null GameObject after menu transition");
            yield break;
        }

        // Wait for the next frame to ensure the UI is fully active
        yield return new WaitForEndOfFrame();

        // Now set the selected UI element
        SetSelected(selectedObject);
    }

    private IEnumerator DelayedSelect(GameObject obj)
    {
        yield return null; // Wait one frame

        if (EventSystem.current == null)
        {
            Debug.LogWarning("No EventSystem found in scene");
            yield break;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
        Debug.Log("Selected UI element: " + obj.name);
    }

    private IEnumerator SelectAfterEndOfFrame(GameObject obj)
    {
        yield return new WaitForEndOfFrame(); // Wait until the end of the frame

        if (EventSystem.current == null)
        {
            Debug.LogWarning("No EventSystem found in scene");
            yield break;
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(obj);
        Debug.Log("Selected after full frame: " + obj.name);
    }
}