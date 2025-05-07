using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para carregar novas cenas

public class Interaction : MonoBehaviour
{
    public float interactionRange = 2f; // Distância máxima para interação
    public LayerMask interactableLayer; // Camada de objetos interativos
    private bool canInteract = false; // Determina se o jogador pode interagir
    private GameObject currentInteractableObject = null; // Referência ao objeto interativo

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        // Verifica a distância do jogador para objetos interativos
        CheckForInteractable();

        // Se o jogador estiver perto de um objeto interativo e pressionar a tecla espaço
        if (canInteract && Input.GetKeyDown(KeyCode.Space))
        {
            InteractWithObject();
        }
    }

    void CheckForInteractable()
    {
        // Realiza um "raycast" (raio) para verificar a presença de um objeto interativo
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, interactionRange, interactableLayer);

        if (hit.collider != null)
        {
            // Se houver um objeto interativo dentro do alcance, permite interação
            canInteract = true;
            currentInteractableObject = hit.collider.gameObject;
        }
        else
        {
            // Caso contrário, não há interação disponível
            canInteract = false;
            currentInteractableObject = null;
        }
    }

    void InteractWithObject()
    {
        if (currentInteractableObject != null)
        {
            // Verifica o tipo do objeto interativo
            if (currentInteractableObject.CompareTag("Porta"))
            {
               // Interage com a porta - muda de cena
               // Debug.Log("Interagiu com a Porta!");
               // SceneManager.LoadScene("NomeDaCena"); // Substitua "NomeDaCena" pelo nome da cena que deseja carregar
            }
            else if (currentInteractableObject.CompareTag("Luz"))
            {
                // Interage com a luz - alterna entre acesa e apagada
                Light lightComponent = currentInteractableObject.GetComponent<Light>();

                if (lightComponent != null)
                {
                    // Se a luz estiver acesa, apaga ela, e vice-versa
                    lightComponent.enabled = !lightComponent.enabled;

                    if (lightComponent.enabled)
                    {
                        Debug.Log("Luz acesa!");
                    }
                    else
                    {
                        Debug.Log("Luz apagada!");
                    }
                }
            }
            
        }
    }

    // Desenha uma linha visível no Editor para visualizar o alcance de interação
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * interactionRange);
    }
}