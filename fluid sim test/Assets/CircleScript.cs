using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
    public float gravity;
    UnityEngine.Vector3[] positions;
    UnityEngine.Vector3[] velocities;
    public int numParticles;
    public float particleSize;
    public float particleSpacing;

    public GameObject particlePrefab;
    public UnityEngine.Vector3 spawn = new UnityEngine.Vector3(0, 0, 0);
    public float smoothingRadius = 1f;


    void Start()
    {
        positions = new UnityEngine.Vector3[numParticles];

        int particlesX = (int)System.Math.Sqrt(numParticles);
        int particlesY = (numParticles -1) / particlesX + 1;
        float spacing = particleSize * 2 + particleSpacing;

        for (int i = 0; i < numParticles; i++)
        {
            float x = (i % particlesX - particlesX / 2f + 0.5f) * spacing;
            float y = (i / particlesX - particlesY / 2f + 0.5f) * spacing;
            float z = UnityEngine.Random.Range(-4f, 4f); 
            positions[i] = new UnityEngine.Vector3(x, y, z);
        }

        for (int i = 0; i < positions.Length; i++)
        {
            Instantiate(particlePrefab, positions[i], UnityEngine.Quaternion.identity);
        }
    }

    static float SmoothingKernal(float radius, float dist)
    {
        float volume = (float)(Math.PI * Math.Pow(radius, 8) / 4);
        float value = Math.Max(0, radius * radius - dist * dist);
        return value * value * value / volume;
    }

    float CalculateDensity(UnityEngine.Vector3 samplePoint)
    {
        float density = 0; 
        const float mass = 1;

        foreach (UnityEngine.Vector3 position in positions)
        {
            float dist = (position - samplePoint).magnitude;
            float influence = SmoothingKernal(smoothingRadius, dist);
            density += mass * influence;
        } 
        return density;
    }

    /* float CalculateProperty(UnityEngine.Vector3 samplePoint)
    {
        float property = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            float dist = (positions[i] - samplePoint).magnitude;
            float influence = SmoothingKernal(smoothingRadius, dist);
            float density = CalculateDensity(positions[i]);
            property += influence * mass;
        }
    } */

}
