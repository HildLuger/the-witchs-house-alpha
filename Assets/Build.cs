using UnityEngine;

public class BuildableObject : MonoBehaviour
{
    public Transform buildPosition; // Posi��o alvo para a constru��o (por exemplo, no n�vel 0)
    public float undergroundY = -5f; // Posi��o inicial abaixo do solo
    public float buildDuration = 3f; // Dura��o da anima��o de constru��o (3 segundos)

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isBuilding = false;
    private float buildTimer = 0f;
    private bool constructionFinished = false;

    private void Start()
    {
        // Configura a posi��o inicial e a posi��o alvo
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

            // Move o objeto do subsolo para a posi��o de constru��o ao longo do tempo
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            if (buildTimer >= buildDuration)
            {
                isBuilding = false;
                constructionFinished = true;
                GameManager.Instance.ConstructionCompleted(); // Informa ao GameManager que a constru��o foi conclu�da
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
