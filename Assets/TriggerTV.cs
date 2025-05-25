using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Video;

public class TriggerTV : MonoBehaviour
{

    public GameObject TVScreen;
    bool triggered = false;
    [SerializeField] float countDown;
    float initialCountDown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TVScreen = GameObject.Find("TVScreen");
        initialCountDown = countDown;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            Light TVLight = GameObject.Find("TVRayLight").GetComponent<Light>();
            VideoPlayer videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
            if (TVScreen != null && TVLight != null)
            {
                
                
                TVLight.enabled = true;
                triggered = true;
                videoPlayer.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && countDown > 0f)
        {
            // for some weird reson the screen first plays a random segment and then the real video
            if (countDown > initialCountDown-1f && countDown < initialCountDown-0.3f)
            {
                MeshRenderer screen = TVScreen.GetComponent<MeshRenderer>();
                screen.enabled = true;
            }

            countDown -= Time.deltaTime;
        }
        else if (triggered && countDown <= 0f)
        {
            Light TVLight = GameObject.Find("TVRayLight").GetComponent<Light>();
            VideoPlayer videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
            if (TVScreen != null && TVLight != null)
            {
                TVLight.enabled = false;
                videoPlayer.Stop();
                MeshRenderer screen = TVScreen.GetComponent<MeshRenderer>();
                screen.enabled = false;
                triggered = false;
                Destroy(this.gameObject);
            }
        }
    }
}
