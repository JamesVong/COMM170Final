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

    private List<GameObject> objectsToMove;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        objectsToMove = new List<GameObject>();
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
        // Collect objects to move before switching the scene
        CollectObjectsToMove();

        // Load the target scene normally
        SceneManager.LoadScene("TestScene");

        // Start the coroutine to switch objects to the new scene
        StartCoroutine(MoveObjectsToTestScene());
    }

    private void CollectObjectsToMove()
    {
        // Get reference to the buildScene
        Scene buildScene = SceneManager.GetActiveScene();

        // Iterate through all root GameObjects in the buildScene
        foreach (GameObject obj in buildScene.GetRootGameObjects())
        {
            // Check if the GameObject or any of its children have a CharacterJoint component
            if (ContainsCharacterJoint(obj))
            {
                // Add the object to the list of objects to move
                objectsToMove.Add(obj);
            }
        }
    }

    private IEnumerator MoveObjectsToTestScene()
    {
        // Wait for the new scene to be fully loaded
        yield return new WaitForEndOfFrame();

        // Get reference to the new active scene
        Scene testScene = SceneManager.GetActiveScene();

        // Move the collected objects to the new active scene
        foreach (GameObject obj in objectsToMove)
        {
            SceneManager.MoveGameObjectToScene(obj, testScene);
        }

        Debug.Log("Moving to new scene.");
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
