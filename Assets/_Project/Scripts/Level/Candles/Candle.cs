using UnityEngine;

public class Candle : Interactable
{
    PlayerItemHandler playerItemHandler;
    bool isLit = false;
    Light candleLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerItemHandler = FindFirstObjectByType<PlayerItemHandler>();
        candleLight = GetComponentInChildren<Light>();
        if (candleLight != null)
        {
            candleLight.enabled = false; // Start with the light off
        }
    }

    public override void interact()
    {
        // if playeritemhandler.getCarriedObject() is candle turn on light
        if (playerItemHandler.GetCarriedObject() != null && playerItemHandler.GetCarriedObject().CompareTag("Candle") && !isLit)
        {
            isLit = true;
            candleLight.enabled = true; // Turn on the light
        }
    }
}
