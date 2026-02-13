using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [SerializeField] float floatAmplitude = 0.25f;
    [SerializeField] float floatSpeed = 2f;

    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

}
