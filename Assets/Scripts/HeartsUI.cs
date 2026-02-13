using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public Player_Health playerHealth;
    public Image heart1;
    public Image heart2;
    public Image heart3;

    void Update()
    {
        if (playerHealth == null) return;

        int hp = playerHealth.CurrentHealth();

        heart1.enabled = hp >= 1;
        heart2.enabled = hp >= 2;
        heart3.enabled = hp >= 3;
    }
}
