using UnityEngine;

public class CapsuleColliderAdjustment : MonoBehaviour
{
    /*[Header("Bone References")]
    public Transform headTopEnd;                   // mixamorig:HeadTop_End
    public Transform leftToeEnd;                   // mixamorig:LeftToe_End
    public Transform rightToeEnd;                  // mixamorig:RightToe_End

    private CapsuleCollider cap;
    private PlayerStateManager player;

    private float defaultX;
    private float defaultZ;
    private float defaultHeight;
    private float defaultCenterY;*/

    void Start()
    {
        /*cap = GetComponent<CapsuleCollider>();
        player = GetComponent<PlayerStateManager>();

        defaultX = cap.center.x;
        defaultZ = cap.center.z;
        defaultHeight = cap.height;
        defaultCenterY = cap.center.y;*/
    }

    void LateUpdate()
    {
        /*var current = player.getCurrentState;

        if (current == player.jumpState || current == player.fallState)
        {
            AdjustFootToHead();
        }
        else
        {
            ResetToDefault();
        }*/
    }

    /// <summary>
    /// Passt Collider an: von Fußspitze bis Kopf, Mittelpunkt = exakt dazwischen.
    /// </summary>
    void AdjustFootToHead()
    {
        /*float footY = Mathf.Min(leftToeEnd.position.y, rightToeEnd.position.y);
        float headY = headTopEnd.position.y;

        float height = Mathf.Max(0.1f, headY - footY);
        float centerYWorld = (headY + footY) * 0.5f;
        float centerYLocal = transform.InverseTransformPoint(new Vector3(transform.position.x, centerYWorld, transform.position.z)).y;

        cap.height = Mathf.Max(height, cap.radius * 2f);
        cap.center = new Vector3(defaultX, centerYLocal, defaultZ);*/
    }

    /// <summary>
    /// Setzt den Collider auf seine Startwerte zurück.
    /// </summary>
    void ResetToDefault()
    {
        /*cap.height = defaultHeight;
        cap.center = new Vector3(defaultX, defaultCenterY, defaultZ);*/
    }
}
