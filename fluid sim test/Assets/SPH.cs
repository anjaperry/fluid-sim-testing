using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Numerics;

[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct Particle
{
    public float pressure;
    public float density;
    public UnityEngine.Vector3 currentForce;
    public UnityEngine.Vector3 velocity; 
    public UnityEngine.Vector3 position; 
}

public class SPH : MonoBehaviour
{
    public bool showSpheres = true;
    public Vector3Int numToSpawn = new Vector3Int(10, 10, 10);
    private int totalParticles
    {
        get
        {
            return numToSpawn.x * numToSpawn.y * numToSpawn.z;
        }
    }
    public UnityEngine.Vector3 boxSize = new UnityEngine.Vector3(5, 10, 5);
    public UnityEngine.Vector3 spawnCenter;
    public float particleRadius = 0.1f;

    public Mesh particleMesh;
    public float particleRenderSize = 8.0f;
    public Material material;
    public ComputeShader shader;
    public Particle[] particles;
    private ComputeBuffer _argsBuffer;
    private ComputeBuffer _particleBuffer;

    private static readonly int SizeProperty = Shader.PropertyToID("_Size");
    private static readonly int ParticleBufferProperty = Shader.PropertyToID("_ParticleBuffer");


    private void Awake()
    {
        SpawnParticles();

        uint[] args = 
        {
            particleMesh.GetIndexCount(0),
            (uint)totalParticles,
            particleMesh.GetIndexStart(0),
            particleMesh.GetBaseVertex(0),
            0
        };

        _argsBuffer = new ComputeBuffer(1,args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        _argsBuffer.SetData(args);

        _particleBuffer = new ComputeBuffer(totalParticles,44);
        _particleBuffer.SetData(particles); 
    }

    private void SpawnParticles()
    {
        UnityEngine.Vector3 spawnPoint = spawnCenter;
        List<Particle> _particles = new List<Particle>();

        for (int i = 0; i < numToSpawn.x; i++)
        {
            for (int j = 0; j < numToSpawn.y; j++)
            {
                for (int k = 0; k < numToSpawn.z; k++)
                {
                    UnityEngine.Vector3 spawnPos = spawnPoint + new UnityEngine.Vector3(i*particleRadius*2, j*particleRadius*2, k*particleRadius*2);
                    Particle p = new Particle
                    {
                        position = spawnPos,
                    }; 

                    _particles.Add(p);
                }
            }
        }

        particles = _particles.ToArray();
    }

    private void OnDrawGizmo()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(UnityEngine.Vector3.zero, boxSize);

        if (!Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(spawnCenter, 0.1f);
        }
    }

    private void Update()
    {
        material.SetFloat(SizeProperty, particleRenderSize);
        material.SetBuffer(ParticleBufferProperty, _particleBuffer);

        if (showSpheres)
        {
            Graphics.DrawMeshInstancedIndirect
            (
                particleMesh, 
                0, 
                material, 
                new Bounds(UnityEngine.Vector3.zero, boxSize), 
                _argsBuffer, 
                castShadows: UnityEngine.Rendering.ShadowCastingMode.Off
            );   
        }
    }
}
