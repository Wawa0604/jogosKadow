using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField] private Button playButton; // Atribua o botão "Play" no Inspector

    public void BotaoPressionado()
    {
        Debug.Log("O botão Play foi pressionado (script Play).");
        // Chama a função IniciarCaraCoroa no GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.IniciarCaraCoroa();
        }
        else
        {
            Debug.LogError("Instância do GameManager não encontrada!");
        }

        // Desativa (faz sumir) o botão Play
        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        // Garante que o botão Play esteja atribuído
        if (playButton == null)
        {
            Debug.LogError("Botão 'Play' não atribuído no script Play!");
            return;
        }

        // Adiciona o listener diretamente aqui
        playButton.onClick.AddListener(BotaoPressionado);
    }
}