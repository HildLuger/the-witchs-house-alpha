using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Velocidade de movimento
    public float rotationSpeed = 300f; // Velocidade de rota��o
    public float acceleration = 2f; // Velocidade de acelera��o para a transi��o entre andar e correr
    public GameObject broom; // Refer�ncia ao objeto da vassoura

    private Animator animator;
    private Rigidbody rb;

    private float moveInput;
    private float rotateInput;
    private float movementValue = 0f; // Para armazenar o valor interpolado do movimento

    private bool canMove = true; // Vari�vel para controlar quando o personagem pode se mover
    private bool isDead = false; // Vari�vel para checar se o jogador est� morto

    void Start()
    {
        animator = GetComponent<Animator>(); // Refer�ncia ao Animator
        rb = GetComponent<Rigidbody>(); // Refer�ncia ao Rigidbody
        broom.SetActive(false); // Garantir que a vassoura esteja invis�vel no in�cio
    }

    void Update()
    {
        if (isDead) return; // Se o jogador est� morto, sair do Update

        // Verificar se o personagem est� em uma anima��o de "Build" ou "Collect"
        bool isBuilding = animator.GetCurrentAnimatorStateInfo(0).IsName("Build");
        bool isCollecting = animator.GetCurrentAnimatorStateInfo(0).IsName("Collect");

        // Desabilitar o movimento se estiver construindo ou coletando
        canMove = !(isBuilding || isCollecting);

        // Se o personagem pode se mover, capturar inputs de movimenta��o
        if (canMove)
        {
            moveInput = Input.GetAxis("Vertical"); // W/S ou Up/Down (valores entre -1 e 1)
            rotateInput = Input.GetAxis("Horizontal"); // A/D ou Left/Right

            // Atualizar o par�metro de movimento no Animator
            UpdateMovement();
        }
        else
        {
            // Se o movimento estiver desabilitado, parar a movimenta��o
            moveInput = 0f;
            rotateInput = 0f;
            UpdateMovement();
        }

        // Verificar teclas para anima��es de coleta e constru��o
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
        if (isDead) return; // Se o jogador est� morto, sair do FixedUpdate

        // Aplicar a movimenta��o f�sica apenas se o personagem puder se mover
        if (canMove)
        {
            MoveCharacter();
            RotateCharacter();
        }
    }

    void UpdateMovement()
    {
        // Definir o valor alvo do movimento baseado no input
        float targetMovementValue = moveInput; // Agora inclui valores negativos para andar para tr�s

        // Interpola��o suave entre o valor atual e o alvo (transi��o suave)
        movementValue = Mathf.Lerp(movementValue, targetMovementValue, acceleration * Time.deltaTime);

        // Atualiza o par�metro "Movement" no Animator com o valor interpolado
        animator.SetFloat("Movement", movementValue);
    }

    void MoveCharacter()
    {
        // Ajustar a velocidade para andar para tr�s mais lentamente
        float adjustedSpeed = moveInput < 0 ? speed / 4f : speed;

        // Mover o personagem para frente e para tr�s
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
        if (!isDead) // Verifica se o jogador ainda n�o est� morto
        {
            isDead = true;
            canMove = false;
            broom.SetActive(false); // Desativa a vassoura ao morrer
            animator.SetTrigger("Death"); // Aciona a anima��o de morte
        }
    }
}
