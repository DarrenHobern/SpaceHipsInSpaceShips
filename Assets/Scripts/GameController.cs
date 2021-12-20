using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private PlayerShip playerPrefab;
    [SerializeField] private ObjectSpawner[] spawners;

    private PlayerShip player;

    public void SetScoreText(int score) {
        scoreText.text = score.ToString();
    }
}
