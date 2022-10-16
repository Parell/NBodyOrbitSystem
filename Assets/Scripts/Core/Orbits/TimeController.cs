using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    public string time;
    public float timeSeconds;
    public int timeScale = 1;
    public int maxTimeScale = 1024;
    public bool useKeys = true;

    private TimeSpan timeSpan;

    private void OnGUI()
    {
        GUI.Label(new Rect(850, 10, 400, 40), time + " x" + timeScale.ToString());
    }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        timeSeconds += timeScale * Time.deltaTime;

        timeSpan = TimeSpan.FromSeconds(timeSeconds);

        time = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s:{4:D3}ms",
                timeSpan.Days,
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds,
                timeSpan.Milliseconds);

        if (useKeys)
        {
            if (Input.GetKeyUp(KeyCode.Slash))
            {
                timeScale = 0;
            }
            else if (Input.GetKeyUp(KeyCode.Comma))
            {
                if (timeScale == 0)
                {
                    timeScale = 1;
                }
                else
                {
                    timeScale *= 2;
                }
                timeScale = Mathf.Clamp(timeScale, 0, maxTimeScale);
            }
            else if (Input.GetKeyUp(KeyCode.Period))
            {
                timeScale /= 2;
                timeScale = Mathf.Clamp(timeScale, 0, maxTimeScale);
            }
        }
    }
}
