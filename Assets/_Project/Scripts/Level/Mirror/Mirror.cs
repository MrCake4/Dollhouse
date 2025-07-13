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
            Debug.LogError("❗ Reflection point is not set on Mirror: " + gameObject.name);
            return null;
        }

        return reflectionPoint;
    }

}
