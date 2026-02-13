using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour
{
    private bool busterActive = false;
    private bool freezeActive = false;

    public bool IsBusterActive() => busterActive;

    public void ActivateBuster(float duration)
    {
        if (!busterActive)
            StartCoroutine(BusterRoutine(duration));
    }

    private IEnumerator BusterRoutine(float duration)
    {
        busterActive = true;
        Debug.Log("Buster Blade Activated");

        yield return new WaitForSeconds(duration);

        busterActive = false;
        Debug.Log("Buster Blade Ended");
    }

    public void ActivateFreeze(float duration)
    {
        if (!freezeActive)
            StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        freezeActive = true;

        Enemy_AI[] enemies = FindObjectsOfType<Enemy_AI>();

        foreach (Enemy_AI e in enemies)
            e.SetFrozen(true);

        Debug.Log("Enemies Frozen");

        yield return new WaitForSeconds(duration);

        enemies = FindObjectsOfType<Enemy_AI>();

        foreach (Enemy_AI e in enemies)
            e.SetFrozen(false);

        freezeActive = false;

        Debug.Log("Freeze Ended");
    }
}
