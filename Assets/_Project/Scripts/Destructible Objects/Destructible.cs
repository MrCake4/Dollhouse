using UnityEngine;

public class Destructible : MonoBehaviour
{

    [SerializeField] GameObject destructibleObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void destroyObject()
    {
        if (destructibleObject != null)
        {
            destructibleObject.transform.localScale = transform.localScale;
            Instantiate(destructibleObject, transform.position, transform.rotation);
            // math this objects scale
            
            // destroy this object
            Destroy(gameObject);
        }
    }
}
