using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int numParticlesPerAxis; 
    public Vector3 center;
    public float size;
    public Vector3 initialVelocity;
    public float jitterStrength; 
    public bool showSpawnBounds;

    public int debug_numParticles;

    public SpawnData GetSpawnData()
    {
        int numPoints = numParticlesPerAxis * numParticlesPerAxis * numParticlesPerAxis;
        Vector3[] points = new Vector3[numPoints];
        Vector3[] velocities = new Vector3[numPoints];

        int i = 0;

        for (int x = 0; x < numParticlesPerAxis; x++)
        {
            for (int y = 0; y < numParticlesPerAxis; y++)
            {
                for (int z = 0; z < numParticlesPerAxis; z++)
                {
                    float tx = (float)x / (numParticlesPerAxis - 1f);
                    float ty = (float)y / (numParticlesPerAxis - 1f);
                    float tz = (float)z / (numParticlesPerAxis - 1f);

                    float px = (tx - 0.5f) * size + center.x;
                    float py = (ty - 0.5f) * size + center.y;
                    float pz = (tz - 0.5f) * size + center.z;
                    Vector3 jitter = UnityEngine.Random.insideUnitSphere * jitterStrength;
                    points[i] = new Vector3(px, py, pz) + jitter;
                    velocities[i] = initialVelocity;
                    i++;
                }
            }
        }

        return new SpawnData() { points = points, velocities = velocities };
    }

    public struct SpawnData
    {
        public Vector3[] points;
        public Vector3[] velocities;
    }

    void OnValidate()
    {
        debug_numParticles = numParticlesPerAxis * numParticlesPerAxis * numParticlesPerAxis;
    }

    void OnDrawGizmos()
    {
        if (showSpawnBounds && !Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, new Vector3(size, size, size));
        }
    }
}
