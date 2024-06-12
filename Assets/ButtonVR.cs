using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.001f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.0116f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }

    public void printPressed()
    {
        Debug.Log("Button pressed.");
    }

    public void SwitchScene()
    {
        // Load the target scene additively to move objects there
        SceneManager.LoadScene("TestScene", LoadSceneMode.Additive);

        // Start the coroutine to switch objects to the new scene
        StartCoroutine(MoveObjectsToTestScene());
    }

    private IEnumerator MoveObjectsToTestScene()
    {
        // Wait for the "TestScene" to be fully loaded
        yield return new WaitUntil(() => SceneManager.GetSceneByName("TestScene").isLoaded);

        // Get references to the scenes
        Scene buildScene = SceneManager.GetSceneByName("BuildScene");
        Scene testScene = SceneManager.GetSceneByName("TestScene");

        // Iterate through all root GameObjects in the buildScene
        foreach (GameObject obj in buildScene.GetRootGameObjects())
        {
            // Check if the GameObject or any of its children have a CharacterJoint component
            if (ContainsCharacterJoint(obj))
            {
                // Move the entire top-level GameObject to the testScene
                SceneManager.MoveGameObjectToScene(obj, testScene);
            }
        }

        // Unload the original scene if needed (optional)
        // SceneManager.UnloadSceneAsync("BuildScene");

        // Finally, set the active scene to the testScene
        SceneManager.SetActiveScene(testScene);
    }

    private bool ContainsCharacterJoint(GameObject parent)
    {
        // Check if the parent or any of its children have a CharacterJoint component
        if (parent.GetComponent<CharacterJoint>() != null)
        {
            return true;
        }

        foreach (Transform child in parent.transform)
        {
            if (ContainsCharacterJoint(child.gameObject))
            {
                return true;
            }
        }

        return false;
    }

}
