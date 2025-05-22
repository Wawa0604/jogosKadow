using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button caraButton;
    [SerializeField] private Button coroaButton;
    [SerializeField] private Button playButton;
    public TextMeshProUGUI guideText;
    [SerializeField] private Button continuarButton; // Certifique-se de ter este botão na cena e referenciado

    private int jogadorDaVez = 0; // 0 para Jogador 1, 1 para Jogador 2
    private int escolhaJogador1 = -1; // -1 = não escolhido, 0 = Cara, 1 = Coroa
    private int resultadoRandom = -1;

    public string jogadorA; // Tornando público
    public string jogadorB; // Tornando público

    public static GameManager Instance;

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

    void Start()
    {
        // Encontra os botões usando SerializeField (configure no Inspector)
        if (caraButton == null || coroaButton == null || playButton == null || guideText == null || continuarButton == null)
        {
            Debug.LogError("Um ou mais elementos de UI não foram atribuídos no GameManager!");
            return;
        }

        // Configuração inicial da UI
        caraButton.gameObject.SetActive(false);
        coroaButton.gameObject.SetActive(false);
        continuarButton.gameObject.SetActive(false); // Inicialmente desativado
        guideText.text = "Pressione PLAY para iniciar.";

        // Adiciona listener ao botão Play para iniciar o Cara ou Coroa
        playButton.onClick.AddListener(IniciarCaraCoroa);
        continuarButton.onClick.AddListener(ContinuarAposResultado);
    }

    public void IniciarCaraCoroa()
    {
        Debug.Log("Botão PLAY pressionado.");
        playButton.gameObject.SetActive(false);
        caraButton.gameObject.SetActive(true);
        coroaButton.gameObject.SetActive(true);
        guideText.text = "Para decidir o primeiro a montar sua equipe, Jogador 1, decida se é Cara ou Coroa.";
        jogadorDaVez = 0; // Jogador 1 começa decidindo
    }

    public void EscolherCara()
    {
        if (jogadorDaVez == 0 && escolhaJogador1 == -1)
        {
            Debug.Log("Jogador 1 escolheu Cara.");
            escolhaJogador1 = 0;
            DecidirQuemComeca();
        }
    }

    public void EscolherCoroa()
    {
        if (jogadorDaVez == 0 && escolhaJogador1 == -1)
        {
            Debug.Log("Jogador 1 escolheu Coroa.");
            escolhaJogador1 = 1;
            DecidirQuemComeca();
        }
    }

    void DecidirQuemComeca()
    {
        if (escolhaJogador1 != -1)
        {
            resultadoRandom = Random.Range(0, 2); // 0 para Cara, 1 para Coroa
            Debug.Log("Resultado do Cara ou Coroa: " + (resultadoRandom == 0 ? "Cara" : "Coroa"));

            if (escolhaJogador1 == resultadoRandom)
            {
                jogadorA = "Jogador 1";
                jogadorB = "Jogador 2";
                guideText.text = " Jogador 1 venceu e será o primeiro a escolher seus campeões";
            }
            else
            {
                jogadorA = "Jogador 2";
                jogadorB = "Jogador 1";
                guideText.text = " Jogador 2 venceu e será o primeiro a escolher seus campeões";
            }

            // Desliga os botões de escolha
            caraButton.gameObject.SetActive(false);
            coroaButton.gameObject.SetActive(false);

            // Ativa o botão continuar
            continuarButton.gameObject.SetActive(true);
        }
    }

    void ContinuarAposResultado()
    {
        Debug.Log("Botão Continuar pressionado.");
        // Desliga o botão continuar
        continuarButton.gameObject.SetActive(false);

        // Chama a próxima etapa do jogo (inicia a seleção de cartas no CartaManager)
        Debug.Log("Próxima etapa: Seleção do primeiro integrante por " + jogadorA);
        guideText.text = jogadorA + ", escolha seu primeiro integrante.";

        // Chama a função no CartaManager para exibir as cartas
        if (CartasManager.Instance != null)
        {
            CartasManager.Instance.InicializarSelecaoCartas(jogadorA, jogadorB);
        }
        else
        {
            Debug.LogError("CartaManager.Instance não encontrado!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}