using UnityEngine;

public class rotatingDummyCylinder : MonoBehaviour
{

    Transform[] dummyTransforms;
    [SerializeField] int rotationSpeed = 30;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dummyTransforms = GetComponentsInChildren<Transform>();

        // randomly make rotation either clockwise or counter-clockwise
        if (Random.Range(0, 3) == 0) rotationSpeed = -rotationSpeed; // Reverse the rotation direction
    }

    // Update is called once per frame
    void Update()
    {
        RotateDummies();
        RotateCylinder();
    }
    void RotateCylinder()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void RotateDummies()
    {
        foreach (Transform dummy in dummyTransforms)
        {
            dummy.Rotate(Vector3.up, rotationSpeed * 2 * Time.deltaTime);
        }
    }
}
