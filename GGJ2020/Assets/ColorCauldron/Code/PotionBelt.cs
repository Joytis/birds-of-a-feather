using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

namespace ColorCauldron {

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class PotionBelt : MonoBehaviour {

    [SerializeField] BasePotion[] potions = null;

    Renderer _renderer;
	
    void Awake() {
        _renderer = GetComponent<Renderer>();
        _renderer.sharedMaterials = potions.Select((potion) => potion.Material).ToArray();
    }

}
} // ColorCauldron
