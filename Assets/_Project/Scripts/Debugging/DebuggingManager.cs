using UnityEngine;

public class DebuggingManager : MonoBehaviour
{
    GameObject[] debugElements;
    private bool isMenuActive = true;

    void Awake()
    {
        debugElements = GameObject.FindGameObjectsWithTag("Debugging");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftShift))
        {
            isMenuActive = !isMenuActive;
            toggleMenu(isMenuActive);
        }
    }

    private void toggleMenu(bool enable)
    {
        foreach (var obj in debugElements)
        {
            obj.SetActive(enable);
        }
    }

    public void toggleDoll(bool isDollEnabled)
    {
        AIStateManager doll = FindAnyObjectByType<AIStateManager>();
        if (!isDollEnabled)
        {
            doll.enabled = false;
        }
        else
        {
            doll.enabled = true;
        }
    }
}
