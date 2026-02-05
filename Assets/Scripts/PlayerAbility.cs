using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public bool bladeActive = false;

    [SerializeField] float bladeDuration = 5f;
    [SerializeField] float freezeDuration = 3f;

    public void ActivateBlade()
    {
        StartCoroutine(BladeRoutine());
    }

    System.Collections.IEnumerator BladeRoutine()
    {
        bladeActive = true;
        Debug.Log("Buster Blade active!");
        yield return new WaitForSeconds(bladeDuration);
        bladeActive = false;
    }

    public void ActivateFreeze()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
            e.Freeze(freezeDuration);

        Debug.Log("Encryption Blast used!");
    }
}
