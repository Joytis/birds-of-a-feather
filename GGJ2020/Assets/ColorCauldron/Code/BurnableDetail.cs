using UnityEngine;

namespace ColorCauldron
{

[System.Serializable]
public class BurnableDetail {
    public static int _BurnEdgeColorId = Shader.PropertyToID("_BurnEdgeColor");
    public static int _BurnEdgeThiccnessId = Shader.PropertyToID("_BurnEdgeThiccness");
    public static int _BurnDissolveAmountId = Shader.PropertyToID("_BurnDissolveAmount");

    public Color edgeColor = new Color(1.0f, 0.5f, 0.0f);    
    public float edgeThiccness = 0.0f;
    public float dissolveAmount = 0.0f;
}

}
