using Unity.VisualScripting;
using UnityEngine;

public class IKSnap : MonoBehaviour
{
    public bool useIK;

    public bool leftHandIK;
    public bool rightHandIK;


    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    public Quaternion leftHandRot;


    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Left Hand Ray
        Debug.DrawRay(transform.position + new Vector3(0f, 2f, 0.5f), -transform.up + new Vector3(-0.5f, 0f, 0f), Color.green);

        //Right Had Ray
        Debug.DrawRay(transform.position + new Vector3(0f, 2f, 0.5f), -transform.up + new Vector3(0.5f, 0f, 0f), Color.green);
    }

    void FixedUpdate()
    {
        RaycastHit LHit;
        RaycastHit RHit;

        //left hand IK check
        if (Physics.Raycast(transform.position + new Vector3(0f, 2f, 0.5f), -transform.up + new Vector3(-0.5f, 0f, 0f), out LHit, 1f))
        {
            leftHandIK = true;
            leftHandPos = LHit.point;
            leftHandRot = Quaternion.FromToRotation(Vector3.up, transform.forward);
        }
        else
        {
            leftHandIK = false;
        }


        //Right Hand IK check
        if (Physics.Raycast(transform.position + new Vector3(0, 2, 0.5f), -transform.up + new Vector3(0.5f, 0, 0), out RHit, 1f))
        {
            rightHandIK = true;
            rightHandPos = RHit.point;
        }
        else
        {
            rightHandIK = false;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (useIK)
        {
            if (leftHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
            }

            if (rightHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                anim.SetIKPosition(AvatarIKGoal.RightHand, leftHandPos);
            }
        }
    }
}
