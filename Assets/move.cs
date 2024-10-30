using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Velocidade de movimento
    public float rotationSpeed = 300f; // Velocidade de rotação
    public float acceleration = 2f; // Velocidade de aceleração para a transição entre andar e correr
    public GameObject broom; // Referência ao objeto da vassoura

    private Animator animator;
    private Rigidbody rb;

    private float moveInput;
    private float rotateInput;
    private float movementValue = 0f; // Para armazenar o valor interpolado do movimento

    private bool canMove = true; // Variável para controlar quando o personagem pode se mover
    private bool isDead = false; // Variável para checar se o jogador está morto

    void Start()
    {
        animator = GetComponent<Animator>(); // Referência ao Animator
        rb = GetComponent<Rigidbody>(); // Referência ao Rigidbody
        broom.SetActive(false); // Garantir que a vassoura esteja invisível no início
    }

    void Update()
    {
        if (isDead) return; // Se o jogador está morto, sair do Update

        // Verificar se o personagem está em uma animação de "Build" ou "Collect"
        bool isBuilding = animator.GetCurrentAnimatorStateInfo(0).IsName("Build");
        bool isCollecting = animator.GetCurrentAnimatorStateInfo(0).IsName("Collect");

        // Desabilitar o movimento se estiver construindo ou coletando
        canMove = !(isBuilding || isCollecting);

        // Se o personagem pode se mover, capturar inputs de movimentação
        if (canMove)
        {
            moveInput = Input.GetAxis("Vertical"); // W/S ou Up/Down (valores entre -1 e 1)
            rotateInput = Input.GetAxis("Horizontal"); // A/D ou Left/Right

            // Atualizar o parâmetro de movimento no Animator
            UpdateMovement();
        }
        else
        {
            // Se o movimento estiver desabilitado, parar a movimentação
            moveInput = 0f;
            rotateInput = 0f;
            UpdateMovement();
        }

        // Verificar teclas para animações de coleta e construção
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Collect");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            animator.SetTrigger("Build");
        }

        // Controlar a visibilidade da vassoura com base no valor de "Movement" no Animator
        ControlBroomVisibility();
    }

    void FixedUpdate()
    {
        if (isDead) return; // Se o jogador está morto, sair do FixedUpdate

        // Aplicar a movimentação física apenas se o personagem puder se mover
        if (canMove)
        {
            MoveCharacter();
            RotateCharacter();
        }
    }

    void UpdateMovement()
    {
        // Definir o valor alvo do movimento baseado no input
        float targetMovementValue = moveInput; // Agora inclui valores negativos para andar para trás

        // Interpolação suave entre o valor atual e o alvo (transição suave)
        movementValue = Mathf.Lerp(movementValue, targetMovementValue, acceleration * Time.deltaTime);

        // Atualiza o parâmetro "Movement" no Animator com o valor interpolado
        animator.SetFloat("Movement", movementValue);
    }

    void MoveCharacter()
    {
        // Ajustar a velocidade para andar para trás mais lentamente
        float adjustedSpeed = moveInput < 0 ? speed / 4f : speed;

        // Mover o personagem para frente e para trás
        Vector3 moveDirection = transform.forward * moveInput * adjustedSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    void RotateCharacter()
    {
        // Rotacionar o personagem para esquerda/direita
        float rotation = rotateInput * rotationSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, rotation, 0f));
    }

    void ControlBroomVisibility()
    {
        // Ativar a vassoura sempre que o personagem estiver se movendo para frente
        if (moveInput > 0.99)
        {
            broom.SetActive(true); // Ativa a vassoura
        }
        else
        {
            broom.SetActive(false); // Desativa a vassoura
        }
    }

    public void Die()
    {
        if (!isDead) // Verifica se o jogador ainda não está morto
        {
            isDead = true;
            canMove = false;
            broom.SetActive(false); // Desativa a vassoura ao morrer
            animator.SetTrigger("Death"); // Aciona a animação de morte
        }
    }
}
