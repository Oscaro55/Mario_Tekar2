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
    private Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        Timer = GameObject.Find("Scrollbar").GetComponent<SpeedCounter>();
        text = GameObject.Find("TimeDif").GetComponent<Text>();
        outline = GameObject.Find("TimeDif").GetComponent<Outline>();
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
            outline.effectColor = Color.green;
        }
        else if (bestTimeValue < currentTimeValue)
        {
            text.text = "+ " + (currentTimeValue - bestTimeValue).ToString("f2");
            StartCoroutine(ResetText());
            outline.effectColor = Color.red;
        }
    }

    IEnumerator ResetText()
    {
        yield return new WaitForSeconds(2);
        text.text = "";
    }
}
