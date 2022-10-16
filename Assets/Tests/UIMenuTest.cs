using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuTest : MonoBehaviour
{
    public Slider slider1;
    public InputField inputField1;
    public InputField inputField2;
    float prog = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            slider1.value = 0;
        }

        slider1.maxValue = Int64.Parse(inputField2.text);

        prog = prog + slider1.value * Time.deltaTime;

        inputField1.text = prog.ToString();
    }
}
