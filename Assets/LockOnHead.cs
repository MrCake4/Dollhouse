using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class AttachToBone : MonoBehaviour
{
    // This script is used to attach a GameObject to a specific bone in any rig.
    [Description("Simply drag and drop the GameObject you want to attach to a bone in the inspector.")]
    [SerializeField] GameObject targetBone; // The bone to which the GameObject will be attached.
    [SerializeField] Vector3 offset;
    [SerializeField] bool alignRotation = false; // If true, the GameObject will align its rotation with the bone's rotation.

void LateUpdate()
{
    if (targetBone != null)
    {
        // Maintain position offset from the bone
        transform.position = targetBone.transform.position + targetBone.transform.rotation * offset;

        // Match rotation if desired
        if (alignRotation)
            transform.rotation = targetBone.transform.rotation;
    }
}
}
