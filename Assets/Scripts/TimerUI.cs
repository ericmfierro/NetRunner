using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public DeliveryManager deliveryManager;
    public TMP_Text timerText;

    float elapsedTime = 0f;
    bool timerRunning = true;

    void Update()
    {
        if (!timerRunning)
            return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = "Time: " + minutes.ToString("00") + ":" + seconds.ToString("00");

        if (deliveryManager != null && deliveryManager.AllDeliveriesComplete())
        {
            timerRunning = false;
            Debug.Log("Level Complete! Final Time: " + timerText.text);
        }
    }
}
