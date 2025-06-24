using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static SceneFadeManager instance;
    [SerializeField] private Image fadeOutImage;
    [Range(0.1f, 10f), SerializeField] private float fadeOutSpeed = 5f;
    [Range(0.1f, 10f), SerializeField] private float fadeInSpeed = 5f;

    [SerializeField] private Color fadeOutStartColor;

    public bool isFadingOut { get; private set; }
    public bool isFadingIn { get; private set; }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        fadeOutStartColor.a = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingOut)
        {
            if (fadeOutImage.color.a < 1f)
            {
                fadeOutStartColor.a += Time.deltaTime * fadeOutSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                isFadingOut = false;
            }
        }

        if (isFadingIn)
        {
            if (fadeOutImage.color.a > 0f)
            {
                fadeOutStartColor.a -= Time.deltaTime * fadeInSpeed;
                fadeOutImage.color = fadeOutStartColor;
            }
            else
            {
                isFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        fadeOutImage.color = fadeOutStartColor;
        isFadingOut = true;
    }

    public void StartFadeIn()
    {
        if (fadeOutImage.color.a >= 1f)
        {
            fadeOutImage.color = fadeOutStartColor;
            isFadingIn = true;
        } 
    }
}
