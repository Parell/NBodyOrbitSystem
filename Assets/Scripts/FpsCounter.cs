using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FpsCounter : MonoBehaviour
{
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    private string m_Text;
    private UIDocument uiDocument;
 
    private void Start()
    {
        uiDocument = GetComponent<UIDocument>();
    }
 
    private void Update()
    {
        uiDocument.rootVisualElement.Q<Label>("FpsLabel").text = m_Text;

        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;
 
        if(timer <= 0) avgFramerate = (int) (1f / timelapse);
        m_Text = string.Format(display,avgFramerate.ToString());
    }
}
