using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] Transform reflectionPoint; // The point where the laser reflects to

    void Awake()
    {
        reflectionPoint = transform.Find("ReflectionPoint");
    }

    public Transform getReflectionPoint()
    {

        if (reflectionPoint == null)
        {
            return null;
        }

        return reflectionPoint;
    }
    
    public void setReflectionPoint(Transform newPoint)
    {
        reflectionPoint = newPoint;
    }

}
