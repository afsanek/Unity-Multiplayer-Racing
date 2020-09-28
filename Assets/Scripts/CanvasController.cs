using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;
    [SerializeField]
    private Text currentLapText;
    
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
    }

    public void InitializeText()
    {
        currentLapText.text = "" + 0;
    }

    public void SetCurrentLap(int value)
    {
        currentLapText.text = "" + value;
    }
}
