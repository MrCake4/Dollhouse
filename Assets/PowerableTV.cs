using UnityEngine;

public class PowerableTV : Interactable
{
    [SerializeField] bool isOn = false;
    [SerializeField] GameObject tvScreen;
    PlayerStateManager player;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerStateManager>();
    }

    public override void interact() { }

    public override void onPowerOff()
    {
        isOn = false;
    }

    public override void onPowerOn()
    {
        isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        if(isOn) tvScreen.SetActive(true);
        else tvScreen.SetActive(false);

        // if player is far away, turn off the tv even if it is on
        if (Vector3.Distance(player.transform.position, transform.position) < 20f)
        {
            if (isOn) tvScreen.SetActive(true);
            return;
        }
        else
        {
            // if player is far away, turn off the TV
            if (isOn) tvScreen.SetActive(false);
            return;
        }
    }
}
