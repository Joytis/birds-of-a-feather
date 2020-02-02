using UnityEngine;

namespace ColorCauldron
{

[System.Serializable]
public class PotionDetail {
    public Gradient magicSpices = new Gradient();
    public Color colorTint = Color.white;    
    public Color specularColor = Color.white;
    public float shininess = 0.5f;
}

}
