using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EngineStats {
    public float MaxSpeed;
    public float Acceleration;
    public float TurnSpeed;
}

public class Engine : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;

    [SerializeField] private EngineStats stats;
    public EngineStats EngineStats => stats;
}
