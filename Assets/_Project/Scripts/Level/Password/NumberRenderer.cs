using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class NumberRenderer : MonoBehaviour
{
    [Header("Settings")]
    [Range(0, 9)]
    public int numberToRender = 0;

    [Tooltip("The number texture (should be arranged 0–4 top row, 5–9 bottom row)")]
    public Texture numberTexture;

    private Material numberMaterial;

    void Start()
    {
        SetupMaterial();
        UpdateUVs(numberToRender);
    }

    void OnValidate()
    {
        if (Application.isPlaying && numberMaterial != null)
        {
            UpdateUVs(numberToRender);
        }
    }

    void SetupMaterial()
    {
        // Create a unique material instance with transparency support
        numberMaterial = new Material(Shader.Find("Unlit/Transparent"))
        {
            mainTexture = numberTexture
        };
        GetComponent<MeshRenderer>().material = numberMaterial;
    }

    public void UpdateUVs(int number)
    {
        if (number < 0 || number > 9) return;

        // Grid: 5 columns x 2 rows
        int column = number % 5;
        int row = number / 5;

        Vector2 tiling = new Vector2(1f / 5f, 0.5f);
        Vector2 offset = new Vector2(column * tiling.x, 1f - ((row + 1) * tiling.y));

        numberMaterial.mainTextureScale = tiling;
        numberMaterial.mainTextureOffset = offset;
    }
}
