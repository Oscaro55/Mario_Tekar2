using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedCounter : MonoBehaviour
{
    public Player player;
    public Scrollbar bar;
    public Text text;
    public Text bestLapText;
    private float timer;
    private float bestLap = 99999;
    private bool once;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.end)
        {
            timer += Time.deltaTime;
            once = true;
        }
        bar.size = player.rb.velocity.magnitude / 50;
        text.text = "Time : " + timer.ToString("f2");
        if (player.end)
        {

            if (timer < bestLap && once)
            {
                bestLap = timer;
                bestLapText.text = "Best Lap : " + bestLap.ToString("f2");
                timer = 0;
                once = false;
            }
            else
            {
                timer = 0;
                once = false;
            }
        }
    }
}
