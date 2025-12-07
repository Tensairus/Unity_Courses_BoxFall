using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private float _defaultAlphaLevel = 1f;

    public void ChangeColorToRandom(Material material)
    {
        material.color = PickRandomColor();
    }

    public void SetColor(Material material, Color color)
    {
        material.color = color;
    }

    private Color PickRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, _defaultAlphaLevel);
    }
}