using UnityEngine;

public class GameController : MonoBehaviour
{
    public double universalTime { get; private set; }
    public bool useTimeScale;
    private int timeScale;
    public int maxTimeScale;

    private void Start()
    {

    }

    private void Update()
    {
    }

    private void HandleTimeScale()
    {
        if (!useTimeScale) { return; }

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

    private void IncreaseTimeScale()
    {
        
    }
}
