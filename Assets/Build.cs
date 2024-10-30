using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public Transform buildPosition; // Posição alvo para a construção (por exemplo, no nível 0)
    public float undergroundY = -5f; // Posição inicial abaixo do solo
    public float buildDuration = 3f; // Duração da animação de construção (3 segundos)

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isBuilding = false;
    private float buildTimer = 0f;
    private bool constructionFinished = false;

    private void Start()
    {
        // Configura a posição inicial e a posição alvo
        startPosition = new Vector3(transform.position.x, undergroundY, transform.position.z);
        targetPosition = new Vector3(transform.position.x, buildPosition.position.y, transform.position.z);
        transform.position = startPosition;
    }

    private void Update()
    {
        if (isBuilding && !constructionFinished)
        {
            buildTimer += Time.deltaTime;
            float progress = buildTimer / buildDuration;

            // Move o objeto do subsolo para a posição de construção ao longo do tempo
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            if (buildTimer >= buildDuration)
            {
                isBuilding = false;
                constructionFinished = true;
                GameManager.Instance.ConstructionCompleted(); // Informa ao GameManager que a construção foi concluída
            }
        }

        // Verifica se o jogador pode construir e pressionou Enter
        if (Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.CanBuild())
        {
            StartConstruction();
            GameManager.Instance.UseItemsForConstruction();
        }
    }

    public void StartConstruction()
    {
        isBuilding = true;
        buildTimer = 0f;
    }
}
