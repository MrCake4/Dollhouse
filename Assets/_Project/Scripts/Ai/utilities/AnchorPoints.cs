using UnityEngine;


public class AnchorPoints : MonoBehaviour
{
    [Header("Override Settings")]
    public bool overrideScanSettings = false;

    public float customViewRadius = 20f;
    public float customSweepDuration = 3f;
    public float customRotationAngle = 90f;
    public float customXRotation = 0f;



    public bool HasOverride => overrideScanSettings;
}