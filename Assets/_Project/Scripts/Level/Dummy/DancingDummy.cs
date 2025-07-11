using UnityEngine;

public class DancingDummy : MonoBehaviour
{

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (anim == null)return;

        anim.SetBool("dancing", true); // Set the dummy to dancing state
        anim.SetInteger("Pose", Random.Range(0, 4)); // Set a random pose between 0 and 2
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
