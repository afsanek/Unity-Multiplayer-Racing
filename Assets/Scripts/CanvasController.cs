using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;
    [SerializeField] private Text currentLapText;
    [SerializeField] private Text totalLapText;
    [SerializeField] private Text endGameController;
    [SerializeField] private GameObject endGamePanel;

    private void Awake()
    {
        if (instance ==null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InitializeText();
    }
    private void Reset()
    {
        currentLapText = GameObject.Find("CurrentLap").GetComponent<Text>();
        currentLapText = GameObject.Find("TotalLap").GetComponent<Text>();
    }
    public void InitializeText()
    {
        currentLapText.text = "" + 0;
        totalLapText.text = "/" + LevelTarget.instance.totalLap;
    }
    public void SetCurrentLap(int value)
    {
        currentLapText.text = "" + value;
    }
    public void SetTotalLap(int value)
    {
        totalLapText.text = "/" + value;
    }

    public void SetEndGameText(string winners)
    {
        endGamePanel.SetActive(true);
        endGameController.text = winners;
    }
}
