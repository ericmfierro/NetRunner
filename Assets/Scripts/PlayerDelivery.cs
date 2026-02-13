using UnityEngine;

public class PlayerDelivery : MonoBehaviour
{
    int deliveredCount = 0;

    public void IncrementDelivered()
    {
        deliveredCount++;
    }

    public int DeliveredCount()
    {
        return deliveredCount;
    }
}
