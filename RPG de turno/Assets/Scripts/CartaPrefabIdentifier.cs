using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartaPrefabIdentifier : MonoBehaviour
{
    public GameObject prefabOriginal; // Arraste o prefab original aqui no Inspector

    private void OnMouseDown()
    {
        // Garante que o clique foi na área do collider
        if (CartasManager.Instance != null)
        {
            CartasManager.Instance.SelecionarCarta(prefabOriginal);
        }
        else
        {
            Debug.LogError("CartaClicavel: CartaManager.Instance é nulo. Certifique-se de que o CartaManager está na cena.");
        }
    }
}