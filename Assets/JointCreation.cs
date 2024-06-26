using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointCreation : MonoBehaviour
{
    public float detectionRadius = 0.5f; // Radius within which the joints will be detected
    public LayerMask jointLayerMask; // LayerMask to specify which objects can form joints

    private void Start()
    {
        // Mark the top-level container as DontDestroyOnLoad
        MarkTopLevelContainer();
    }

    private void Update()
    {
        // Check for nearby JointLocators within the specified LayerMask
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, jointLayerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.gameObject.layer == 8 && collider.transform.parent != transform.parent)
            {
                CreateCharacterJoint(collider.gameObject);
                break;
            }
        }
    }

    private void CreateCharacterJoint(GameObject target)
    {
        // Check if the joint already exists
        if (GetComponent<CharacterJoint>() == null)
        {
            CharacterJoint joint = gameObject.AddComponent<CharacterJoint>();

            joint.connectedBody = target.GetComponent<Rigidbody>();

            // Optionally, configure the joint's properties
            joint.anchor = Vector3.zero; // Adjust the anchor position if necessary
            joint.autoConfigureConnectedAnchor = false; // Automatically configure the connected anchor
            joint.enableProjection = true;
            joint.projectionDistance = 0;
            // You can further customize the joint properties like limits, springs, etc. here

            Debug.Log($"Created CharacterJoint between {gameObject.name} and {target.name}");

            // Ensure the top-level container is marked as DontDestroyOnLoad
            MarkTopLevelContainer();
        }
    }

    private void MarkTopLevelContainer()
    {
        // Get the top-level parent of this object
        Transform topLevelParent = transform;
        while (topLevelParent.parent != null)
        {
            topLevelParent = topLevelParent.parent;
        }

        // Mark the top-level parent as DontDestroyOnLoad
        DontDestroyOnLoad(topLevelParent.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to visualize the detection radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
