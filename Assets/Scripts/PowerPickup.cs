using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    public enum PowerupType { Blade, Freeze }
    public PowerupType type;

    [SerializeField] float duration = 5f;
    [SerializeField] GameObject pickupFX;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerAbility ability = other.GetComponent<PlayerAbility>();
        if (ability == null)
            return;

        if (type == PowerupType.Blade)
            ability.ActivateBuster(duration);

        if (type == PowerupType.Freeze)
            ability.ActivateFreeze(duration);

        SpawnFX();

        Destroy(gameObject);
    }

    /*void SpawnFX()
    {
        if (pickupFX == null)
            return;

        GameObject fx = Instantiate(pickupFX);
        fx.transform.position = transform.position;

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Play();
    }*/
    void SpawnFX()
    {
        Debug.Log("SpawnFX called");

        if (pickupFX == null)
        {
            Debug.Log("pickupFX is NULL");
            return;
        }

        GameObject fx = Instantiate(pickupFX);
        fx.transform.position = transform.position;

        Debug.Log("FX Instantiated at: " + fx.transform.position);

        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Play();
    }

}
