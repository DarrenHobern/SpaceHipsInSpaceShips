using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[SelectionBase]
public abstract class Ship : MonoBehaviour, ILaserable
{
    #region Fields & Properties
    [SerializeField] protected float health = 100f;
    [SerializeField] protected int score = 50;
    [SerializeField] protected new Rigidbody rigidbody;

    [SerializeField] protected List<Engine> engines;

    [BoxGroup("Lasers")] [SerializeField] // Ingore these, just unity tags
        private MiningLaser miningLaserPrefab;
    [BoxGroup("Lasers")] [SerializeField]
        private AttackingLaser attackingLaserPrefab;
    [BoxGroup("Lasers")] [SerializeField]
        protected Transform[] laserSlots; // places to spawn lasers

    // Because we don't care about the types of lasers, we can have a collection of all of them.
    // More likely though we need to do specific logic on the mining lasers vs the attacking ones,
    // so typically you'd have a collection for each, and possibly a helper function to build the collection of both as needed.
    // Basically its a performance vs memory optimization question
    protected List<Laser> lasers;
    protected EngineStats totalEngineStats;
    protected float velocity = 0f;
    #endregion

    #region Initialization
    // virtual is just marking this as a function that can be overridden by a subclass
    protected virtual void Initialize() {
        InitializeLasers();
        InitializeEngines();
    }

    private void Start() {
        Initialize();
    }

    /// <summary>
    /// Randomly assigns lasers into the slots
    /// </summary>
    private void InitializeLasers() {
        lasers = new List<Laser>();

        // foreach (Transform t in laserSlots) {
        for (int i = 0; i < laserSlots.Length; i++) {
            Transform slot = laserSlots[i];
            bool isMining = i % 2 == 0;
            Laser laser;
            if (isMining)
            {
                laser = Instantiate<Laser>(miningLaserPrefab, slot);
            } else {
                laser = Instantiate<Laser>(attackingLaserPrefab, slot);
            }
            lasers.Add(laser);
        }
    }

    /// <summary>
    /// Calculates the stats based on the engines equipped
    /// </summary>
    private void InitializeEngines() {
        foreach (Engine e in engines) {
            totalEngineStats.Acceleration += e.EngineStats.Acceleration;
            totalEngineStats.MaxSpeed += e.EngineStats.MaxSpeed;
            totalEngineStats.TurnSpeed += e.EngineStats.TurnSpeed;
        }
    }
    #endregion

    protected void UpdateRotation(Vector3 target) {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, totalEngineStats.TurnSpeed * Time.deltaTime);
    }

    #region Death and Dying
    /// <summary>
    /// This exists because its an ILaserable object its going to do something on hit
    /// </summary>
    /// <param name="amount"></param>
    public virtual bool Damage(float amount, out int score) {
        health -= amount;
        if (health <= 0f) {
            OnDeath();
            score = this.score;
            return true;
        }
        score = 0;
        return false;
    }

    protected virtual void OnDeath() {
        print($"I have been destroyed: {name}");
        Destroy(gameObject);
    }
    #endregion
}

