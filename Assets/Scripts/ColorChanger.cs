using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void ChangeColorToRandom(Material material)
    {
        material.color = PickRandomColor();
    }

    private Color PickRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1.0f);
    }
}