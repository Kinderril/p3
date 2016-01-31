using System;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
    public class ParticleSystemMultiplier : MonoBehaviour
    {
        // a simple script to scale the size, speed and lifetime of a particle system
        private ParticleSystem[] systems;
        public float multiplier = 1;

        void Awake()
        {

            systems = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem system in systems)
            {
                system.enableEmission = false;
            }

        }
        public void StartPlay()
        {
            foreach (ParticleSystem system in systems)
            {
                system.enableEmission = true;
                system.startSize *= multiplier;
                system.startSpeed *= multiplier;
                system.startLifetime *= Mathf.Lerp(multiplier, 1, 0.5f);
                system.Clear();
                system.Play();
            }
        }
    }
}
