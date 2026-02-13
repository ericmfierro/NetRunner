using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    [SerializeField] List<GameObject> deliveryTargets = new List<GameObject>();

    int currentIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ShuffleDeliveries();
    }

    void ShuffleDeliveries()
    {
        for (int i = 0; i < deliveryTargets.Count; i++)
        {
            int randomIndex = Random.Range(i, deliveryTargets.Count);

            GameObject temp = deliveryTargets[i];
            deliveryTargets[i] = deliveryTargets[randomIndex];
            deliveryTargets[randomIndex] = temp;
        }
    }

    public GameObject GetCurrentTarget()
    {
        if (currentIndex >= deliveryTargets.Count)
            return null;

        return deliveryTargets[currentIndex];
    }

    public void CompleteDelivery()
    {
        currentIndex++;
    }

    public bool AllDeliveriesComplete()
    {
        return currentIndex >= deliveryTargets.Count;
    }

    public int GetDeliveredCount()
    {
        return currentIndex;
    }

    public int GetTotalDeliveries()
    {
        return deliveryTargets.Count;
    }
}
