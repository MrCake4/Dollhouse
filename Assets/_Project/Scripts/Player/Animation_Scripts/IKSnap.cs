using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class IKSnap : MonoBehaviour
{
    /*public bool useIK;

    public bool leftHandIK;
    public bool rightHandIK;

    public bool leftFootIK;
    public bool rightFootIK;


    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    public Vector3 leftHandOriginalPos;
    public Vector3 rightHandOriginalPos;


    public Vector3 leftFootPos;
    public Vector3 rightFootPos;


    public Vector3 leftHandOffset;
    public Vector3 rightHandOffset;

    public Vector3 leftFootOffset;
    public Vector3 rightFootOffset;


    public Quaternion leftHandRot;
    public Quaternion rightHandRot;

    public Quaternion leftFootRot;
    public Quaternion rightFootRot;

    public Quaternion leftFootRotOffset;
    public Quaternion rightFootRotOffset;


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

        //Left Foot
        Debug.DrawRay(transform.position + new Vector3(-0.4f, 0.4f, 0), transform.forward, Color.red);
        //Right Foot
        Debug.DrawRay(transform.position + new Vector3(0.4f, 0.4f, 0), transform.forward, Color.red);

    }

    void FixedUpdate()
    {
        RaycastHit LHit;
        RaycastHit RHit;

        RaycastHit LFHit;
        RaycastHit RFHit;

        //left hand IK check
        if (Physics.Raycast(transform.position + new Vector3(0f, 2f, 0.5f), -transform.up + new Vector3(-0.5f, 0f, 0f), out LHit, 1f))
        {
            leftHandIK = true;
            leftHandPos = LHit.point - leftHandOffset;
            //leftHandPos.x = leftHandOriginalPos.x;
            leftHandPos.z = leftFootPos.z - leftHandOffset.z;
            leftHandRot = Quaternion.FromToRotation(Vector3.forward, LHit.normal);
        }
        else
        {
            leftHandIK = false;
        }


        //Right Hand IK check
        if (Physics.Raycast(transform.position + new Vector3(0, 2, 0.5f), -transform.up + new Vector3(0.5f, 0, 0), out RHit, 1f))
        {
            rightHandIK = true;
            rightHandPos = RHit.point - rightHandOffset;
            //rightHandPos.x = rightHandOriginalPos.x;
            rightHandPos.z = rightFootPos.z - rightHandOffset.z;
            rightHandRot = Quaternion.FromToRotation(Vector3.forward, RHit.normal);
        }
        else
        {
            rightHandIK = false;
        }


        //Left Foot IK check
        if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(-0.35f, 0.4f, 0)), transform.forward, out LFHit, 1))
        {
            leftFootIK = true;
            leftFootPos = LFHit.point - leftFootOffset;
            leftFootRot = (Quaternion.FromToRotation(Vector3.up, LFHit.normal)) * leftFootRotOffset;
        }
        else
        {
            leftFootIK = false;
        }

        //Right Foot IK check
        if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.35f, 0.4f, 0)), transform.forward, out RFHit, 1))
        {
            rightFootIK = true;
            rightFootPos = RFHit.point - rightFootOffset;
            rightFootRot = (Quaternion.FromToRotation(Vector3.up, RFHit.normal)) * leftFootRotOffset;
        }
        else
        {
            rightFootIK = false;
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (useIK)
        {
            leftHandOriginalPos = anim.GetIKPosition(AvatarIKGoal.LeftHand);
            rightHandOriginalPos = anim.GetIKPosition(AvatarIKGoal.RightHand);

            if (leftHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);

                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandRot);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            }

            if (rightHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                anim.SetIKPosition(AvatarIKGoal.RightHand, leftHandPos);

                anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandRot);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            }

            if (leftFootIK)
            {
                anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);

                anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRot);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            }

            if (rightFootIK)
            {
                anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);

                anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
            }
        }
    }*/
}
