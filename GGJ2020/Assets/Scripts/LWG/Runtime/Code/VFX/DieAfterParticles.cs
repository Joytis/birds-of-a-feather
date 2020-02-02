using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LWG.VFX 
{
    
[RequireComponent(typeof(ParticleSystem))]
public class DieAfterParticles : MonoBehaviour
{

    ParticleSystem _ps;

    void Awake() => _ps = GetComponent<ParticleSystem>();

    // Update is called once per frame
    void Update() {
        if(!_ps.IsAlive()) {
            Destroy(gameObject);
        }
    }
}

}