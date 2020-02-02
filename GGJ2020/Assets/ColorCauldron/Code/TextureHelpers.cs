using System;
using System.Linq;
using UnityEngine;

namespace ColorCauldron {

public static class TextureHelpers {

    static int TextureWidth => 32;
    static int TextureHeight => 1;

    public static Texture2D CreateTexture(Gradient gradient) {
        var tex = new Texture2D(TextureWidth, TextureHeight, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Point; // NO BLENDING. Always CLEAN colors.  
        // tex.alphaIsTransparency = true; 
        tex.anisoLevel = 1; // We dont' really care about filtering.
        UpdateTextureColors(tex, gradient);
        return tex;
    }

    public static void UpdateTextureColors(Texture2D tex, Gradient gradient) {
        int previousPosition = 0;

        // Create a list of pixel position color pairs. 
        int maxNumColors = gradient.colorKeys.Length;
        int maxNumAlphas = gradient.alphaKeys.Length;

        int currentColorPosition = 0;
        int currentAlphaPosition = 0;

        // Iterate over all permutations of alpha and color. 
        while(currentColorPosition < maxNumColors && currentAlphaPosition < maxNumAlphas) 
        {
            var colorKey = gradient.colorKeys[currentColorPosition];
            var alphaKey = gradient.alphaKeys[currentAlphaPosition];

            // Check which should be incremented. 
            int newPosition;
            if(colorKey.time < alphaKey.time) {
                currentColorPosition += 1;
                newPosition = (int)(Mathf.Round(colorKey.time * tex.width));
            }
            else {
                currentAlphaPosition += 1;
                newPosition = (int)(Mathf.Round(alphaKey.time * tex.width));
            }

            // Generate a color buffer and fill texture. 
            var color = colorKey.color;
            color.a = alphaKey.alpha;
            int blockWidth = newPosition - previousPosition;
            int blockHeight = TextureHeight;
            // Copy buffer
            Color[] buffer = Enumerable.Repeat(color, blockWidth * blockHeight).ToArray();
            tex.SetPixels(previousPosition, 0, blockWidth, blockHeight, buffer);

            // Update position to reflect new texture block. 
            previousPosition = newPosition; 
        }

        tex.Apply();
    }
}

}