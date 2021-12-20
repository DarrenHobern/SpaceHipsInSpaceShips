using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using Shapes;

public abstract class Laser : MonoBehaviour
{
    [SerializeField] private Line line;
    [SerializeField] private new SphereCollider collider;
    [SerializeField] private Transform pivotTransform;
    [Tooltip("Damage ticks per second")]
    [SerializeField] private float rateOfFire;
    [SerializeField] private float damagePerFire = 1f;
    [SerializeField] private float range = 3f;
    [Tag] [SerializeField] private string[] targetTags;

    private float nextFireTime = 0f;
    private bool canFire = false;
    private Coroutine fireCoroutine;

    public Action<int> OnScoreIncrease;

    public void Initialize() {
        collider.radius = range;
    }

    private void Update() {
        if (Time.time >= nextFireTime) {
            canFire = true;
        }
    }

    private void OnTriggerStay(Collider other) {
        // Limit lasers to only target the things they're allowed to:
        // First by tag - eg asteroids for mining lasers
        // then by component, it actually has to be laserable
        if (targetTags.Contains(other.tag)) {
            pivotTransform.rotation = Quaternion.LookRotation(other.transform.position - transform.position, Vector3.up);
            line.End = transform.InverseTransformPoint(other.transform.position);
            // TryFireLaser(other);
        }
    }

    private void TryFireLaser(Collider other)
    {
        if (!canFire) {
            return;
        }
        if (other.TryGetComponent<ILaserable>(out ILaserable laserTarget))
        {
            nextFireTime = Time.time + (1f / rateOfFire);
            canFire = false;
            line.End = other.transform.position;
            Color col = line.Color;
            col.a = 1;
            line.Color = col;
            if (fireCoroutine != null) {
                StopCoroutine(fireCoroutine);
            }
            // fireCoroutine = StartCoroutine(FireLaserCoroutine());
            if (laserTarget.Damage(damagePerFire, out int score))
            {
                OnScoreIncrease?.Invoke(score);
                line.End = line.Start;
            }
        }
    }

    private IEnumerator FireLaserCoroutine() {
        while (line.Color.a > 0) {
            Color c = line.Color;
            c.a -= Time.deltaTime; // this isn't a good way to do this
            line.Color = c;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }
}
