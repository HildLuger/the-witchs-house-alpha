using UnityEngine;
using TMPro; // Importa a biblioteca TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Quantidades de itens
    private int mushrooms = 0;
    private int pumpkins = 0;
    private int bats = 0;

    // UI para mostrar a quantidade de cada item
    public TMP_Text mushroomsText;
    public TMP_Text pumpkinsText;
    public TMP_Text batsText;

    // UI para o temporizador
    public TMP_Text timerText;

    // Quantidade necessária para construir
    public int requiredAmount = 10;

    // Tempo do temporizador
    public float timerDuration = 180f; // Tempo em segundos (3 minutos)
    private float timeRemaining;
    private bool timerRunning = true;
    private bool constructionCompleted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timeRemaining = timerDuration;
        UpdateUI();
    }

    private void Update()
    {
        // Atualiza o temporizador se ele estiver rodando e a construção não foi concluída
        if (timerRunning && !constructionCompleted)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                TriggerPlayerDeath(); // Aciona a morte do jogador
            }

            UpdateTimerUI();
        }
    }

    public void CollectItem(string itemType)
    {
        switch (itemType)
        {
            case "Mushroom":
                mushrooms++;
                break;
            case "Pumpkin":
                pumpkins++;
                break;
            case "Bat":
                bats++;
                break;
        }

        UpdateUI();
    }

    public bool CanBuild()
    {
        return mushrooms >= requiredAmount && pumpkins >= requiredAmount && bats >= requiredAmount;
    }

    public void UseItemsForConstruction()
    {
        if (CanBuild())
        {
            mushrooms -= requiredAmount;
            pumpkins -= requiredAmount;
            bats -= requiredAmount;
            UpdateUI();
        }
    }

    public void ConstructionCompleted()
    {
        constructionCompleted = true;
    }

    private void UpdateUI()
    {
        if (mushroomsText != null)
            mushroomsText.text = "x" + mushrooms;

        if (pumpkinsText != null)
            pumpkinsText.text = "x" + pumpkins;

        if (batsText != null)
            batsText.text = "x" + bats;
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void TriggerPlayerDeath()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.Die();
        }
    }
}
