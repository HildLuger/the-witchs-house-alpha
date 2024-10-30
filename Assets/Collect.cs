using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    public string itemType; // "Mushroom", "Pumpkin" ou "Bat"
    public float destroyDelay = 1f; // Tempo em segundos antes de destruir o objeto ap�s a coleta

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Inicia o processo de coleta se o jogador estiver no Trigger
            StartCoroutine(CollectWhenPressed());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cancela a coleta se o jogador sair do Trigger
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator CollectWhenPressed()
    {
        // Espera at� que o jogador pressione a tecla Espa�o para coletar o item
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        // Verifica se o GameManager.Instance est� definido
        if (GameManager.Instance != null)
        {
            // Coleta o item e atualiza o GameManager
            GameManager.Instance.CollectItem(itemType);

            // Espera o tempo configurado em destroyDelay antes de destruir o objeto
            yield return new WaitForSeconds(destroyDelay);

            // Destr�i o item ap�s o tempo de atraso
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("GameManager instance is missing!");
        }
    }
}
