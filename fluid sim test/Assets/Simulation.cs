using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public event System.Action SimulationStepCompleted;

    public float timeScale = 1;
    public bool fixedTimeStep;
    public int iterationsPerFrame;
    public float gravity = -9.8f;
    [Range(0,1)] public float damping;
    public float smoothingRadius = 0.2f;
    public float targetDensity;
    public float pressureMultiplier;
    public float nearPressureMultiplier;
    public float viscosityStrength; 

    public ComputeShader compute;
    public Spawner spawner;
    public ParticleDisplay display; 
    public Transform floorDisplay;

    public ComputeBuffer positionBuffer { get; private set; }
    public ComputeBuffer velocityBuffer { get; private set; }
    public ComputeBuffer densityBuffer { get; private set; }
    public ComputeBuffer predictedPositionBuffer; 
    ComputeBuffer spatialIndices;
    ComputeBuffer spatialOffsets; 

    const int ExternalForcesKernel = 0;
    const int spatialHashKernel = 1;
    const int densityKernal = 2;
    const int pressureKernal = 3;
    const int viscosityKernal = 4;
    const int updatePositionKernal = 5; 

    GPUSort gpuSort; 

    bool isPaused; 
    bool pauseNextFrame;
    Spawner.SpawnData spawnData;

    void Start()
    {
        float deltaTime = 1 / 60f;
        Time.fixedDeltaTime = deltaTime;

        spawnData = spawner.GetSpawnData();

        int numParticles = spawnData.points.Length;
        //positionBuffer = ComputeHelper.CreateStructuredBuffer<Vector3>(numParticles);

    }

}
