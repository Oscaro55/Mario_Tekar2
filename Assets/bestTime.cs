using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bestTime : MonoBehaviour
{
    private float bestTimeValue;
    private float currentTimeValue;
    private SpeedCounter Timer;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        Timer = GameObject.Find("Scrollbar").GetComponent<SpeedCounter>();
        text = GameObject.Find("TimeDif").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  OnTriggerEnter(Collider other)
    {
        currentTimeValue = Timer.timer;
        if (bestTimeValue == 0) bestTimeValue = currentTimeValue;
        else if (bestTimeValue >= currentTimeValue)
        {
            text.text = "- " + (bestTimeValue-currentTimeValue).ToString("f2");
            bestTimeValue = currentTimeValue;
            StartCoroutine(ResetText());
        }
        else if (bestTimeValue < currentTimeValue)
        {
            text.text = "+ " + (currentTimeValue - bestTimeValue).ToString("f2");
            StartCoroutine(ResetText());
        }
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(2);
        text.text = "";
    }
}
