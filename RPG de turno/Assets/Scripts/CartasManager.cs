using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartasManager : MonoBehaviour
{
    [Header("Referências de Prefabs de Sprite das Cartas")]
    public GameObject cartaPrefabA;
    public GameObject cartaPrefabB;
    public GameObject cartaPrefabC;
    public GameObject cartaPrefabD;

    [Header("Referências de Ícones")]
    public GameObject iconePrefabA;
    public GameObject iconePrefabB;
    public GameObject iconePrefabC;
    public GameObject iconePrefabD;

    [Header("Referências de Locais das Cartas")]
    public Transform place1;
    public Transform place2;
    public Transform place3;
    public Transform place4;

    [Header("Referências de Locais dos Ícones do Jogador 1")]
    public Transform placeIconeJogador1_1;
    public Transform placeIconeJogador1_2;

    [Header("Referências de Locais dos Ícones do Jogador 2")]
    public Transform placeIconeJogador2_1;
    public Transform placeIconeJogador2_2;

    private List<GameObject> cartasInstanciadas = new List<GameObject>();
    private List<GameObject> cartasDisponiveis = new List<GameObject>();
    private int cartasJogador1Selecionadas = 0;
    private int cartasJogador2Selecionadas = 0;
    private string jogadorA; // Quem começa escolhendo
    private string jogadorB; // O outro jogador

    public static CartasManager Instance;

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

    public void InicializarSelecaoCartas(string primeiroJogador, string segundoJogador)
    {
        jogadorA = primeiroJogador;
        jogadorB = segundoJogador;
        ExibirCartasParaEscolha();
    }

    void ExibirCartasParaEscolha()
    {
        // Limpa quaisquer cartas instanciadas anteriormente
        foreach (var carta in cartasInstanciadas)
        {
            if (carta != null)
            {
                Destroy(carta);
            }
        }
        cartasInstanciadas.Clear();
        cartasDisponiveis.Clear();

        // Instancia os prefabs das cartas nos locais designados
        InstantiateCarta(cartaPrefabA, place1);
        InstantiateCarta(cartaPrefabB, place2);
        InstantiateCarta(cartaPrefabC, place3);
        InstantiateCarta(cartaPrefabD, place4);

        // Atualizar o Guide Text no GameManager
        GameManager.Instance.guideText.text = $"{jogadorA}, escolha seu primeiro integrante.";
    }

    void InstantiateCarta(GameObject prefab, Transform parent)
    {
        if (prefab != null && parent != null)
        {
            GameObject carta = Instantiate(prefab, parent.position, parent.rotation);
            cartasInstanciadas.Add(carta);
            cartasDisponiveis.Add(carta);
            CartaPrefabIdentifier clicavel = carta.GetComponent<CartaPrefabIdentifier>();
            if (clicavel != null)
            {
                clicavel.prefabOriginal = prefab;
            }
        }
    }

    public void SelecionarCarta(GameObject cartaPrefabSelecionada)
    {
        GameObject cartaParaDestruir = cartasDisponiveis.Find(carta => carta.GetComponent<CartaPrefabIdentifier>()?.prefabOriginal == cartaPrefabSelecionada);

        if (cartaParaDestruir != null)
        {
            cartasDisponiveis.Remove(cartaParaDestruir);
            Destroy(cartaParaDestruir);

            string jogadorQueSelecionou = (cartasJogador1Selecionadas + cartasJogador2Selecionadas) % 2 == 0 ? jogadorA : jogadorB;

            if (jogadorQueSelecionou == jogadorA)
            {
                InstanciarIcone(cartaPrefabSelecionada, (cartasJogador1Selecionadas == 0) ? placeIconeJogador1_1.position : placeIconeJogador1_2.position);
                cartasJogador1Selecionadas++;
            }
            else if (jogadorQueSelecionou == jogadorB)
            {
                InstanciarIcone(cartaPrefabSelecionada, (cartasJogador2Selecionadas == 0) ? placeIconeJogador2_1.position : placeIconeJogador2_2.position);
                cartasJogador2Selecionadas++;
            }

            AtualizarTextoGuiaSelecao(jogadorQueSelecionou);
        }
    }

    void AtualizarTextoGuiaSelecao(string jogadorQueSelecionou)
    {
        string mensagemGuia;

        if ((jogadorQueSelecionou == jogadorA && cartasJogador1Selecionadas < 2) || (jogadorQueSelecionou == jogadorB && cartasJogador2Selecionadas < 2))
        {
            // Ainda há seleções pendentes
            mensagemGuia = $"Vez de {(jogadorQueSelecionou == jogadorA ? jogadorB : jogadorA)} escolher.";
        }
        else
        {
            // Ambos os jogadores já escolheram seus times
            mensagemGuia = $"{jogadorA} e {jogadorB} escolheram seus times.";
            GameManager.Instance.guideText.text = $"Como {GameManager.Instance.jogadorA} começou escolhendo os times, {GameManager.Instance.jogadorB} dará início às rodadas.";
            // Aqui você pode chamar uma função no GameManager para iniciar a próxima fase
        }

        GameManager.Instance.guideText.text = mensagemGuia;
    }

    void InstanciarIcone(GameObject cartaPrefab, Vector3 position)
    {
        if (cartaPrefab == cartaPrefabA && iconePrefabA != null)
        {
            Instantiate(iconePrefabA, position, Quaternion.identity);
        }
        else if (cartaPrefab == cartaPrefabB && iconePrefabB != null)
        {
            Instantiate(iconePrefabB, position, Quaternion.identity);
        }
        else if (cartaPrefab == cartaPrefabC && iconePrefabC != null)
        {
            Instantiate(iconePrefabC, position, Quaternion.identity);
        }
        else if (cartaPrefab == cartaPrefabD && iconePrefabD != null)
        {
            Instantiate(iconePrefabD, position, Quaternion.identity);
        }
    }
}