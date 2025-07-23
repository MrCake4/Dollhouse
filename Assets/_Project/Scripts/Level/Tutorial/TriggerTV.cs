using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Video;

public class TriggerTV : MonoBehaviour
{

    public GameObject TVScreen;
    bool triggered = false;
    [SerializeField] float countDown;
    AudioSource audioSource;
    [SerializeField] AudioClip TVSound;
    float initialCountDown;
    Light TVLight;
    VideoPlayer videoPlayer;
    MeshRenderer screen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TVScreen = GameObject.Find("TVScreen");
        initialCountDown = countDown;
        TVLight = GameObject.Find("TVLightRay").GetComponent<Light>();
        videoPlayer = GameObject.Find("Video Player").GetComponent<VideoPlayer>();
        screen = TVScreen.GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            if (TVScreen != null && TVLight != null)
            {
                
                if (TVSound != null) audioSource = SoundEffectsManager.instance.PlayLoopedSoundEffect(TVSound, TVScreen.transform, 1f);
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
                screen.enabled = true;
            }

            countDown -= Time.deltaTime;
        }
        else if (triggered && countDown <= 0f)
        {
            if (TVScreen != null && TVLight != null)
            {
                TVLight.enabled = false;
                videoPlayer.Stop();
                screen.enabled = false;
                triggered = false;
                if (TVSound != null) SoundEffectsManager.instance.StopSoundEffect(audioSource);
                Destroy(this.gameObject);
            }
        }
    }
}
