
using UnityEngine;



public class ParticleSystmeAbsorber : BaseEffectAbsorber {

    public ParticleSystem ParticleSystem;

    void Awake()
    {
        if (ParticleSystem == null)
        {
            ParticleSystem = GetComponent<ParticleSystem>();
        }
    }

    public override void Play()
    {
        gameObject.SetActive(true);
//        ParticleSystem.enableEmission = true;
    }

    public override void Stop()
    {

        gameObject.SetActive(false);
//        ParticleSystem.enableEmission = false;
    }
}
