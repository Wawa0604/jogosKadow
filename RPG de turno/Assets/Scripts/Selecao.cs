using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Selecao : MonoBehaviour
{
    public static Selecao Instance;
    public TextMeshProUGUI guideText;
    public Button continuarSelecaoButton;
    public string jogadorSelecionando; // Para controlar de quem é a vez de escolher
    public string jogadorA; // O jogador que começa escolhendo
    public string jogadorB; // O outro jogador
    private int cartasSelecionadasJogadorA = 0;
    private int cartasSelecionadasJogadorB = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Chamado pelo GameManager_CaraCoroa após o "Continuar"
    public void IniciarSelecaoPrimeiroIntegrante(string primeiroJogador, string segundoJogador)
    {
        jogadorA = primeiroJogador;
        jogadorB = segundoJogador;
        jogadorSelecionando = jogadorA; // O jogador A começa

        if (guideText != null)
        {
            guideText.text = $"{jogadorSelecionando}, escolha seu primeiro integrante.";
        }

        if (continuarSelecaoButton != null)
        {
            continuarSelecaoButton.gameObject.SetActive(false); // Só aparece após ambos os jogadores escolherem seus times
            continuarSelecaoButton.onClick.RemoveAllListeners(); // Limpa listeners anteriores
            continuarSelecaoButton.onClick.AddListener(IniciarProximaFase);
        }
        else
        {
            Debug.LogError("Botão 'Continuar Selecao' não atribuído no Selecao!");
        }

        // Informa ao CartaManager para exibir as cartas
        CartasManager.Instance.InicializarSelecaoCartas(jogadorA, jogadorB);
    }

    public void AtualizarTextoGuia(string texto)
    {
        if (guideText != null)
        {
            guideText.text = texto;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI 'guideText' não atribuído no Selecao!");
        }
    }

    public void AlternarJogadorSelecionando()
    {
        if (jogadorSelecionando == jogadorA)
        {
            jogadorSelecionando = jogadorB;
        }
        else
        {
            jogadorSelecionando = jogadorA;
        }
        AtualizarTextoGuia($"Vez de {jogadorSelecionando} escolher.");
    }

    public void CartaSelecionada(string jogador)
    {
        if (jogador == jogadorA)
        {
            cartasSelecionadasJogadorA++;
            if (cartasSelecionadasJogadorA == 2 && cartasSelecionadasJogadorB == 2)
            {
                AtualizarTextoGuia($"Como {jogadorA} começou escolhendo os times, {jogadorB} dará início às rodadas.");
                MostrarBotaoContinuarSelecao();
            }
            else if (cartasSelecionadasJogadorA < 2)
            {
                AtualizarTextoGuia($"Vez de {jogadorB} escolher.");
                jogadorSelecionando = jogadorB;
            }
        }
        else if (jogador == jogadorB)
        {
            cartasSelecionadasJogadorB++;
            if (cartasSelecionadasJogadorA == 2 && cartasSelecionadasJogadorB == 2)
            {
                AtualizarTextoGuia($"Como {jogadorA} começou escolhendo os times, {jogadorB} dará início às rodadas.");
                MostrarBotaoContinuarSelecao();
            }
            else if (cartasSelecionadasJogadorB < 2)
            {
                AtualizarTextoGuia($"Vez de {jogadorA} escolher.");
                jogadorSelecionando = jogadorA;
            }
        }

        // Aqui você também precisaria guardar a carta escolhida para o jogador
    }

    public void MostrarBotaoContinuarSelecao()
    {
        if (continuarSelecaoButton != null)
        {
            continuarSelecaoButton.gameObject.SetActive(true);
            continuarSelecaoButton.onClick.RemoveAllListeners();
            continuarSelecaoButton.onClick.AddListener(IniciarProximaFase);
        }
        else
        {
            Debug.LogError("Botão 'Continuar Seleção' não atribuído no Selecao!");
        }
    }

    void IniciarProximaFase()
    {
        Debug.Log("Seleção de times finalizada. Iniciando a fase de posicionamento/combate.");
        // Aqui você implementaria a lógica para a próxima fase do jogo
        AtualizarTextoGuia($"{jogadorB}, selecione o card do seu primeiro personagem.");
        // E chamaria a lógica para mostrar os cards escolhidos para posicionamento
    }
}