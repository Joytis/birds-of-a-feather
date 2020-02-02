using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace ColorCauldron {

public abstract class BasePotion : ScriptableObject {
    // The current matieral for our potion. 
    public abstract Material Material { get; }

}

}