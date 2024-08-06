using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private Text scoreText;
    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else if (Instance == null && Instance != this)
        {
            Instance = this;
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void Start()
    {
        UpdateScoreText();
    }

    private int pickupsCollected = 0;
    public enum propertyList { Key1 = 1, Key2 = 2, Chest = 10, Golden = 20 }

    public void ModifyPickupAmount(property pick)
    {
        pickupsCollected |= (int)pick.GetProperty();
    }
    public bool ifOpenDoors(property pick)
    {
        if (pick.GetProperty() == propertyList.Key1 || pick.GetProperty() == propertyList.Key2)
        {
            if ((pickupsCollected & (int)pick.GetProperty()) != 0)
            {
                pickupsCollected &= ~(int)pick.GetProperty();
                return true;
            }
        }
        return false;
    }

}
