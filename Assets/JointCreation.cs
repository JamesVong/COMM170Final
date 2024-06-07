using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointCreation : MonoBehaviour
{
    public float detectionRadius = 0.5f; // Radius within which the joints will be detected
    public LayerMask jointLayerMask; // LayerMask to specify which objects can form joints

    private void Update()
    {
        // Check for nearby JointLocators within the specified LayerMask
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, jointLayerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != gameObject) // Ensure we don't create a joint with ourselves
            {
                CreateCharacterJoint(collider.gameObject);
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
            joint.autoConfigureConnectedAnchor = true; // Automatically configure the connected anchor
            // You can further customize the joint properties like limits, springs, etc. here

            Debug.Log($"Created CharacterJoint between {gameObject.name} and {target.name}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to visualize the detection radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

