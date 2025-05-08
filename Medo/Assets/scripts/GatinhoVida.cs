using UnityEngine;
using TMPro;
using Unity.VisualScripting.ReorderableList;
using System;

public class GatinhoVida : MonoBehaviour
{
    public int vidas = 3;
    public TextMeshProUGUI meuTextoVidas; // Para exibir as vidas
    public TextMeshProUGUI meuTextoFome;   // Para exibir a fome
    public string textoInicialVidas = "Vidas: ";
    public string textoInicialFome = "Fome: ";
    public float fome = 60f;
    public float fomeMaxima = 60f;
    public float tempoParaPerderFome = 1f; // Taxa de perda de fome (segundos)
    public AudioClip somPerderVida;
    private AudioSource audioSourceGatinho;
    public AudioClip somComerRatinho;
    //private AudioSource
    private bool estaMorto = false;

    public delegate void VidaPerdidaAction(int vidasRestantes);
    public static event VidaPerdidaAction OnVidaPerdida;

    public delegate void GatinhoMorreuAction();
    public static event GatinhoMorreuAction OnGatinhoMorreu;

    void Start()
    {
        meuTextoVidas = GameObject.Find("VidaGatinho").GetComponent<TextMeshProUGUI>();
        meuTextoFome = GameObject.Find("FomeGatinho").GetComponent<TextMeshProUGUI>(); // Encontre o texto da fome

        if (meuTextoVidas == null || meuTextoFome == null)
        {
            Debug.LogWarning("Componente TextMeshProUGUI 'VidaGatinho' ou 'FomeGatinho' não encontrado na cena!");
        }

        audioSourceGatinho = GetComponent<AudioSource>();
        if (audioSourceGatinho == null)
        {
            audioSourceGatinho = gameObject.AddComponent<AudioSource>();
        }

        AtualizarTextoVidas();
        AtualizarTextoFome();
    }

    void Update()
    {
        if (estaMorto) return;

        // Diminui a fome ao longo do tempo
        fome -= Time.deltaTime;

        // Garante que a fome não seja menor que zero
        fome = Mathf.Max(0f, fome);

        // Atualiza o texto da fome na tela
        AtualizarTextoFome();

        // Verifica se o gato morreu de fome
        if (fome <= 0)
        {
            Debug.Log("Gatinho morreu de fome!");
            estaMorto = true;
            if (OnGatinhoMorreu != null)
            {
                OnGatinhoMorreu();
            }
            Time.timeScale = 0f;
            if (GetComponent<MovimentoGatinho>() != null)
            {
                GetComponent<MovimentoGatinho>().enabled = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (estaMorto) return;

        if (collision.gameObject.CompareTag("Aranha"))
        {
            PerderVida();
        }
        else if (collision.gameObject.CompareTag("Ratinho"))
        {
            ComerRatinho(collision.gameObject);
        }
    }

    public void PerderVida()
    {
        if (estaMorto) return;

        vidas--;
        Debug.Log("Gatinho perdeu uma vida! Vidas restantes: " + vidas);

        if (OnVidaPerdida != null)
        {
            OnVidaPerdida(vidas);
        }

        AtualizarTextoVidas();
        TocarSomPerderVida();

        if (vidas <= 0 && !estaMorto)
        {
            Debug.Log("Gatinho morreu!");
            estaMorto = true;
            if (OnGatinhoMorreu != null)
            {
                OnGatinhoMorreu();
            }
            Time.timeScale = 0f;
            if (GetComponent<MovimentoGatinho>() != null)
            {
                GetComponent<MovimentoGatinho>().enabled = false;
            }
        }
    }

    void ComerRatinho(GameObject ratinhoComido)
    {
        fome = fomeMaxima; // Restaura a fome para o máximo
        AtualizarTextoFome();
        Destroy(ratinhoComido);
        Debug.Log("Gatinho comeu o ratinho! Fome restaurada.");
        TocarSomComerRatinho(); // Chamando a função para tocar o som ao comer o ratinho
        // Adicione aqui a lógica para contar ratinhos comidos, se necessário
    }

    void AtualizarTextoVidas()
    {
        if (meuTextoVidas != null)
        {
            meuTextoVidas.text = textoInicialVidas + vidas.ToString();
        }
    }

    void AtualizarTextoFome()
    {
        if (meuTextoFome != null)
        {
            meuTextoFome.text = textoInicialFome + Mathf.RoundToInt(fome).ToString();
        }
    }

    void TocarSomPerderVida()
    {
        if (audioSourceGatinho != null && somPerderVida != null)
        {
            audioSourceGatinho.PlayOneShot(somPerderVida);
        }
    }

    void TocarSomComerRatinho()
    {
        if (audioSourceGatinho != null && somComerRatinho != null)
        {
            audioSourceGatinho.PlayOneShot(somComerRatinho);
        }
    }

    private void OnDestroy()
    {
        OnVidaPerdida -= OnVidaPerdidaHandler;
    }

    private void OnVidaPerdidaHandler(int vidasRestantes)
    {
        vidas = vidasRestantes;
        AtualizarTextoVidas();
        TocarSomPerderVida();
    }
}