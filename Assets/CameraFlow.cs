using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // O personagem que a câmera deve seguir
    public float smoothSpeed = 0.125f; // Velocidade de suavização

    // Posições iniciais da câmera (baseadas nas coordenadas atuais fornecidas)
    private Vector3 initialPosition = new Vector3(-0.98f, 5.46f, 9.93f);
    private Vector3 offset;

    void Start()
    {
        // Calcular o offset inicial com base na posição inicial e na posição do alvo (personagem)
        offset = initialPosition - target.position;

        // Configurar a rotação inicial da câmera (baseada na rotação fornecida)
        transform.rotation = Quaternion.Euler(35.7f, -149.2f, 0f);
    }

    void LateUpdate()
    {
        // Posição desejada da câmera (acima do personagem com offset)
        Vector3 desiredPosition = target.position + offset;

        // Suavização da transição da câmera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplicar a nova posição da câmera
        transform.position = smoothedPosition;

        // Manter a câmera olhando para o personagem (opcional)
        transform.LookAt(target);
    }
}
