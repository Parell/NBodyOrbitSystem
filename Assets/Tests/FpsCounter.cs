using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    private string m_Text;

    private void OnGUI()
    {
        GUI.Label(new Rect(500, 10, 200, 40), m_Text);
    }
 
    private void Start()
    {
        //m_Text = GetComponent<Text>();
    }
 
    private void Update()
    {
        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;
 
        if(timer <= 0) avgFramerate = (int) (1f / timelapse);
        m_Text = string.Format(display,avgFramerate.ToString());
    }
}
