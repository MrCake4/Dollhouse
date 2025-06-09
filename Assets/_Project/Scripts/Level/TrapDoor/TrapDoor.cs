using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    bool isOpen = false;
    public Transform doorPivot;
    public float openSpeed = 2f; // Speed of the door opening
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private float openProgress = 0f;

    void Start()
    {
        closedRotation = doorPivot.localRotation;
        openRotation = Quaternion.Euler(270, 0, 0); // Target rotation for the door
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key") && !isOpen)
        {
            isOpen = true;
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (isOpen && openProgress < 1f)
        {
            openProgress += Time.deltaTime * openSpeed;
            doorPivot.localRotation = Quaternion.Lerp(closedRotation, openRotation, openProgress);
        }
    }

    // if isOpen is set to true, the door will open
    public void setIsOpen(bool open)
    {
        isOpen = open;
    }
}
