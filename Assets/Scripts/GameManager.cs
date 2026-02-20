using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] Player_Health playerHealth;
    [SerializeField] DeliveryManager deliveryManager;

    [Header("UI")]
    [SerializeField] GameObject endGamePanel;
    [SerializeField] TMP_Text endGameText;

    bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (gameEnded)
            return;

        // LOSE CONDITION
        if (playerHealth.CurrentHealth() <= 0)
        {
            EndGame(false);
        }

        // WIN CONDITION
        if (deliveryManager.AllDeliveriesComplete())
        {
            EndGame(true);
        }
    }

    void EndGame(bool win)
    {
        gameEnded = true;
        Time.timeScale = 0f;

        endGamePanel.SetActive(true);

        if (win)
            endGameText.text = "YOU WIN!";
        else
            endGameText.text = "YOU LOSE!";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
