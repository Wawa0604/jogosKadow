using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    private Button cara;
    private Button coroa;
    private Button play;

    // Start is called before the first frame update
    void Start()
    {
        cara = GameObject.Find("Cara").GetComponent<Button>();
        coroa = GameObject.Find("Coroa").GetComponent<Button>();
        play = GameObject.Find("Play").GetComponent<Button>(); // Corrigido para encontrar o botão "Play"

        cara.gameObject.SetActive(false);
        coroa.gameObject.SetActive(false);
        
    }

    public void BotaoPressionado()
    {
        Debug.Log("O botão Play foi pressionado!");
        // Ativa os botões Cara e Coroa
        if (cara != null)
        {
            cara.gameObject.SetActive(true);
        }
        if (coroa != null)
        {
            coroa.gameObject.SetActive(true);
        }

        // Desativa (faz sumir) o botão Play
        if (play != null)
        {
            play.gameObject.SetActive(false);
        }
    }
}