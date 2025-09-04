using UnityEngine;

public class PauseManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        DebugUtils.DrawDebugText("Time scale", Time.timeScale, Color.blue);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 1 - Time.timeScale;
        }
    }
}
