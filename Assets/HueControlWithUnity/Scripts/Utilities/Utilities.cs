
using UnityEngine;

public class Utilities 
{


    public static void ConvertFromRGBTOPhilipsColors(float r, float g, float b, out float brightness, out float hue, out float saturation)
    {
        Color c = new Color(r, g, b);
        Color.RGBToHSV(c, out hue, out saturation, out brightness);
        hue *= 65535f;
        saturation *= 254f;
        brightness *= 254f;
        brightness = Mathf.Max(brightness, 1f);
    }
}
