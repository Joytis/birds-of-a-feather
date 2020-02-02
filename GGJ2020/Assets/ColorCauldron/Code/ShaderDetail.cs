namespace ColorCauldron
{

public enum SurfaceType
{
    Opaque,
    Transparent
}

public enum BlendMode
{
    Alpha,   // Old school alpha-blending mode, fresnel does not affect amount of transparency
    Premultiply, // Physically plausible transparency mode, implemented as alpha pre-multiply
    Additive,
    Multiply
}

[System.Serializable]
public class ShaderDetail {
    public SurfaceType surfaceType = SurfaceType.Opaque;
    public BlendMode blendMode = BlendMode.Premultiply;
    public bool receiveShadows = false;
    public bool specularHighlights = false;
    public bool enableInstancing = true;
    public bool burnable = true;
}

}