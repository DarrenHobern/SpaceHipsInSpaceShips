using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteriod : MonoBehaviour, ILaserable
{
    [SerializeField] private float health = 20f;
    [SerializeField] private int score = 10;

    public bool Damage(float amount, out int score) {
        health -= amount;
        if (health <= 0f) {
            score = this.score;
            Destroy(gameObject);
            return true;
        }
        score = 0;
        return false;
    }
}
