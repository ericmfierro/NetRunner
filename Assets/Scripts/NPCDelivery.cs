using UnityEngine;

public class NPCDelivery : MonoBehaviour
{
    bool delivered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (delivered)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (DeliveryManager.Instance == null)
            return;

        GameObject currentTarget = DeliveryManager.Instance.GetCurrentTarget();

        if (currentTarget == null)
            return;

        if (currentTarget.transform.root != transform.root)
            return;

        delivered = true;

        PlayerDelivery player = other.GetComponent<PlayerDelivery>();
        if (player != null)
            player.IncrementDelivered();

        DeliveryManager.Instance.CompleteDelivery();
    }
}
