using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the real entry point for all the lasers and engines and things (for the player).
/// </summary>
public class PlayerShip : Ship
{
    [SerializeField] private int cargo; // This is going to be the score

    private new Camera camera;

    protected override void Initialize() {
        base.Initialize();
        camera = Camera.main;
        foreach (Laser l in lasers) {
            l.OnScoreIncrease += OnScoreIncrease;
        }
    }

    # region Player Input
    private void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical");

        UpdateVelocity(vertical);
        Vector3 mouse = Input.mousePosition;
        Vector3 mousePosition = camera.ScreenToWorldPoint(
            new Vector3(mouse.x, mouse.y, camera.transform.position.y));
        UpdateRotation(mousePosition);
    }

    private void UpdateVelocity(float vertical)
    {
        velocity += vertical * totalEngineStats.Acceleration * Time.deltaTime;
        velocity = Mathf.Clamp(velocity, -totalEngineStats.MaxSpeed, totalEngineStats.MaxSpeed);
        transform.Translate(transform.forward * velocity * Time.deltaTime, Space.World);
    }

    #endregion

    private void OnScoreIncrease(int amount) {
        // TODO GUI score or something
    }

    protected override void OnDeath() {
        print("Game Over!");
        Time.timeScale = 0f; // Pause the game, normally its like a dialog or something
    }
}
