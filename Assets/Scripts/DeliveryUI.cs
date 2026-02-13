using UnityEngine;
using TMPro;

public class DeliveryUI : MonoBehaviour
{
    public DeliveryManager deliveryManager;
    public TMP_Text deliveryText;

    void Update()
    {
        int delivered = deliveryManager.GetDeliveredCount();
        int total = deliveryManager.GetTotalDeliveries();

        deliveryText.text = "Deliveries: " + delivered + " / " + total;
    }
}
