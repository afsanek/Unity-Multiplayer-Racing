using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text counter;

    private int _count = 3;
    public bool isCountingDone;

    private void Start()
    {
        //when 4 player join match start counting !
        //StartCounting();
    }

    public void StartCounting()
    {
        StartCoroutine(CountDowner());
    }

    private IEnumerator CountDowner()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (_count == 0)
            {
                counter.text = "GO";
                yield return new WaitForSeconds(1f);
                counter.text = "";
                break;
            }
            counter.text = "" + _count;
            _count--; 
        }
    }
}
