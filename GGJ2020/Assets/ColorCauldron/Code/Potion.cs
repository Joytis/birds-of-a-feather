using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using LWG;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ColorCauldron {

[CreateAssetMenu(fileName = "potion_newPotion", menuName = "LWG/ColorCauldron/Potion")]
public class Potion : BasePotion 
{
	public const float GlossinessMin = 1;
	public const float GlossinessMax = 11;
	public const float SpecularityMin = 0;
	public const float SpecularityMax = 4;

    // Variable Ids
    static int _SpecColorId = Shader.PropertyToID("_SpecColor");
    static int _ShininessId = Shader.PropertyToID("_Shininess");
    static int _ReceiveShadowsId = Shader.PropertyToID("_ReceiveShadows");
    static int _SpecularHighlightsId = Shader.PropertyToID("_SpecularHighlights");
    static int _BaseMapId = Shader.PropertyToID("_BaseMap");
    static int _BaseColorId = Shader.PropertyToID("_BaseColor");

    [SerializeField, HideInInspector] PotionDetail _potionDetail = null;
    [SerializeField, HideInInspector] ShaderDetail _shaderDetail = null;
    // [SerializeField, HideInInspector] BurnableDetail _burnDetail = null;
    [SerializeField, HideInInspector] Shader potionRecipe = null;

    [SerializeField] 
    Material _material = null;

    [SerializeField] 
    Texture2D _texture = null;

    // ================================================================
    // SHADER OPTIONS
    // ================================================================
    public SurfaceType SurfaceType {
        get => _shaderDetail.surfaceType;
        private set {
            _shaderDetail.surfaceType = value;
            MaterialHelpers.SetMaterialBlendMode(Material, _shaderDetail.surfaceType, _shaderDetail.blendMode);
        }
            // _shaderDetail.surfaceType = value;
    }

    public BlendMode BlendMode {
        get => _shaderDetail.blendMode;
        private set {
            _shaderDetail.blendMode = value;
            MaterialHelpers.SetMaterialBlendMode(Material, _shaderDetail.surfaceType, _shaderDetail.blendMode);
        }
            // _shaderDetail.blendMode = value;
    }

    public bool EnableInstancing {
        get => _shaderDetail.enableInstancing;
        private set {
            _shaderDetail.enableInstancing = value;
            Material.enableInstancing = _shaderDetail.enableInstancing;
        }
            // _shaderDetail.enableInstancing = value;
    }

    public bool ReceiveShadows {
        get => _shaderDetail.receiveShadows;
        private set {
            _shaderDetail.receiveShadows = value;
            CoreUtils.SetKeyword(Material, "_RECEIVE_SHADOWS_OFF", !_shaderDetail.receiveShadows);
        }
            // _shaderDetail.receiveShadows = value;
    }

    public bool SpecularHighlights {
        get => _shaderDetail.specularHighlights;
        private set {
            _shaderDetail.specularHighlights = value;
            CoreUtils.SetKeyword(Material, "_SPECULARHIGHLIGHTS_OFF", !_shaderDetail.specularHighlights);
        }
            // _shaderDetail.specularHighlights = value;
    }

    // Basic shader fields. 
    public Gradient MagicSpices {
        get => _potionDetail.magicSpices;
        private set => TextureHelpers.UpdateTextureColors(_texture, _potionDetail.magicSpices);
            // _potionDetail.magicSpices = value;
    }

    public Color ColorTint {
        get => _potionDetail.colorTint;
        private set => Material.SetColor(_BaseColorId, _potionDetail.colorTint);
            // _potionDetail.colorTint = value;
    }

    public Color SpecularColor {
        get => _potionDetail.specularColor;
        private set => Material.SetColor(_SpecColorId, _potionDetail.specularColor);
            // _potionDetail.specularColor = value;
    }


    public float Shininess {
        get => _potionDetail.shininess;
        private set => Material.SetFloat(_ShininessId, _potionDetail.shininess);
            // _potionDetail.shininess = value;
    }

    public Shader PotionRecipe {
        get => potionRecipe;
        // private set => CreateMaterial();
            // potionRecipe = value;
    }

    // [ShowInInspector, ReadOnly]
    public override Material Material => _material;
}

}