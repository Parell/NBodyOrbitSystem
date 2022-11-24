using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool useKeys;
    public int maxTimeScale;
    private int timeScale;

    private void Update()
    {
        if (useKeys)
        {
            if (Input.GetKeyUp(KeyCode.Period))
            {
                timeScale /= 2;
            }
            else if (Input.GetKeyUp(KeyCode.Comma))
            {
                timeScale *= 2;

                if (timeScale == 0)
                {
                    timeScale = 1;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Slash))
            {
                timeScale = 0;
            }

            if (timeScale >= maxTimeScale)
            {
                timeScale = maxTimeScale;
            }
            
            GetComponent<Propagator>().timeScale = timeScale;
        }
    }
}
