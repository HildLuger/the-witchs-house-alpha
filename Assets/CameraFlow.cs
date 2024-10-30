using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // O personagem que a c�mera deve seguir
    public float smoothSpeed = 0.125f; // Velocidade de suaviza��o

    // Posi��es iniciais da c�mera (baseadas nas coordenadas atuais fornecidas)
    private Vector3 initialPosition = new Vector3(-0.98f, 5.46f, 9.93f);
    private Vector3 offset;

    void Start()
    {
        // Calcular o offset inicial com base na posi��o inicial e na posi��o do alvo (personagem)
        offset = initialPosition - target.position;

        // Configurar a rota��o inicial da c�mera (baseada na rota��o fornecida)
        transform.rotation = Quaternion.Euler(35.7f, -149.2f, 0f);
    }

    void LateUpdate()
    {
        // Posi��o desejada da c�mera (acima do personagem com offset)
        Vector3 desiredPosition = target.position + offset;

        // Suaviza��o da transi��o da c�mera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplicar a nova posi��o da c�mera
        transform.position = smoothedPosition;

        // Manter a c�mera olhando para o personagem (opcional)
        transform.LookAt(target);
    }
}
