using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    public enum PowerupType { Blade, Freeze }
    public PowerupType type;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAbility ability = other.GetComponent<PlayerAbility>();

            if (ability != null)
            {
                if (type == PowerupType.Blade)
                    ability.ActivateBlade();

                if (type == PowerupType.Freeze)
                    ability.ActivateFreeze();
            }

            Destroy(gameObject);
        }
    }
}
