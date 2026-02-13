using UnityEngine;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    public Enemy_Spawner spawner;
    public TMP_Text enemyText;

    void Update()
    {
        enemyText.text = "Enemies: " + spawner.CurrentEnemyCount();
    }
}
