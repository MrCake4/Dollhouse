using UnityEngine;

public class TrapDoorColorManager : MonoBehaviour
{
    [SerializeField]PlatformColorCheck[] platformColorChecks;
    TrapDoor trapDoor;

    void Awake()
    {
        trapDoor = GetComponent<TrapDoor>();
    }

    void Update()
    {
        if (platformColorChecks != null)
        {
            if (checkSolved()) trapDoor.setIsOpen(true);
        }
    }
    
    bool checkSolved()
    {
        foreach (var platform in platformColorChecks)
        {
            if (!platform.getSolved)
            {
                return false;
            }
        }
        return true;
    }
}