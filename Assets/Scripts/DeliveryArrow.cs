using UnityEngine;

public class DeliveryArrow : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = transform.parent;
    }

    void Update()
    {
        if (DeliveryManager.Instance == null)
            return;

        GameObject target = DeliveryManager.Instance.GetCurrentTarget();

        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        Vector3 direction = target.transform.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
