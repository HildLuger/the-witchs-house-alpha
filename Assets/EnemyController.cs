using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f; // Velocidade do inimigo
    public float rotationSpeed = 30f; // Velocidade de rotação
    public float rotationChangeInterval = 2f; // Intervalo em segundos para mudar a direção de rotação
    public float alertDistance = 5f; // Distância para tocar o som de alerta

    public AudioClip alertSound; // Som de alerta
    public AudioClip[] collisionSounds; // Array de sons de colisão
    private AudioSource audioSource;
    private Transform player; // Referência ao jogador

    private float rotationDirection = 1f; // Direção da rotação (1 para direita, -1 para esquerda)
    private float rotationTimer;
    private bool alertPlayed = false; // Controla se o som de alerta já foi tocado

    void Start()
    {
        rotationTimer = rotationChangeInterval;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is missing on the Enemy GameObject.");
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' is missing.");
        }
    }

    void Update()
    {
        // Mover o inimigo para frente constantemente
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Girar o inimigo gradualmente na direção atual
        transform.Rotate(0, rotationSpeed * rotationDirection * Time.deltaTime, 0);

        // Contagem regressiva para mudar a direção de rotação
        rotationTimer -= Time.deltaTime;
        if (rotationTimer <= 0)
        {
            // Alterna aleatoriamente a direção de rotação (1 para direita, -1 para esquerda)
            rotationDirection = Random.Range(0, 2) == 0 ? -1f : 1f;

            // Reinicia o temporizador
            rotationTimer = rotationChangeInterval;
        }

        // Verifica a distância entre o inimigo e o jogador
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= alertDistance && !alertPlayed)
            {
                // Toca o som de alerta uma vez quando o jogador está próximo
                if (audioSource != null && alertSound != null)
                {
                    audioSource.PlayOneShot(alertSound);
                }
                alertPlayed = true;
            }
            else if (distanceToPlayer > alertDistance)
            {
                alertPlayed = false; // Reseta o som de alerta quando o jogador se afasta
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se colidiu com o jogador
        if (other.CompareTag("Player"))
        {
            // Toca um som de colisão aleatório
            if (collisionSounds.Length > 0 && audioSource != null)
            {
                int randomIndex = Random.Range(0, collisionSounds.Length);
                audioSource.PlayOneShot(collisionSounds[randomIndex]);
            }

            // Inicia a animação de morte no jogador
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}
