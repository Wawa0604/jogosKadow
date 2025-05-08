using UnityEngine;
using System.Collections;

public class aranhaFollow : MonoBehaviour
{
    public Transform personagem; // Arraste o Transform do gatinho para este campo no Inspector
    public float velocidadeAranha = 5f;
    public float distanciaMinima = 0.5f;   // Distância mínima para parar de seguir
    public bool olharParaJogador = true;
    public float rotacaoVelocidade = 5f;
    private bool colidindoInicial = false; // Usado para detectar a primeira entrada no trigger
    public float tempoDeVidaPerseguindo = 5f; // Tempo em segundos após o qual a aranha morre
    public GameObject efeitoMorte; // Prefab de um efeito visual ao morrer (opcional)
    public AudioClip somIniciarPerseguicao; // Arraste o som de iniciar perseguição para cá no Inspector
    public float fadeOutDelay = 2f; // Tempo em segundos após o início da perseguição para começar o fade out
    public float fadeOutDuration = 1f; // Duração do fade out em segundos
    private AudioSource audioSourceAranha;
    private bool seguindo = false;
    private float tempoInicioPerseguicao;
    private bool jogoAcabou = false;

    private Rigidbody2D rb; // Declarando 'rb' como uma variável de membro da classe

    void OnEnable()
    {
        GatinhoVida.OnGatinhoMorreu += PararAranha;
    }

    void OnDisable()
    {
        GatinhoVida.OnGatinhoMorreu -= PararAranha;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D não encontrado neste GameObject!");
            enabled = false;
            return;
        }

        if (personagem == null)
        {
            GameObject gatinho = GameObject.Find("gatinho");
            if (gatinho != null)
            {
                personagem = gatinho.transform;
            }
            else
            {
                Debug.LogError("Gatinho não encontrado na cena!");
                enabled = false;
                return;
            }
        }

        // Obtém ou adiciona um AudioSource à aranha
        audioSourceAranha = GetComponent<AudioSource>();
        if (audioSourceAranha == null)
        {
            audioSourceAranha = gameObject.AddComponent<AudioSource>();
        }
        audioSourceAranha.volume = 1f; // Inicia o volume no máximo
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (jogoAcabou) return; // Se o jogo acabou, não processa mais triggers

        if (other.CompareTag("Player"))
        {
            colidindoInicial = true; // O jogador entrou no raio de ativação
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (jogoAcabou) return; // Se o jogo acabou, não processa mais triggers

        if (other.CompareTag("Player"))
        {
            colidindoInicial = false;
        }
    }

    void Update()
    {
        if (jogoAcabou) return; // Se o jogo acabou, não executa a lógica normal

        // Se o jogador estiver (ou esteve) dentro do raio inicial e pressionar Espaço, inicie a perseguição
        if (colidindoInicial && Input.GetKeyDown(KeyCode.Space) && !seguindo)
        {
            seguindo = true;
            tempoInicioPerseguicao = Time.time; // Registra o tempo de início da perseguição
            StartCoroutine(AutodestruirAposTempo());
            TocarSomIniciarPerseguicao(); // Inicia o som
            StartCoroutine(FadeOutAudioAfterDelay()); // Inicia o fade out após um delay
        }

        // Se estiver seguindo, move e rotaciona
        if (seguindo)
        {
            SeguirGatinhoDireto(); // Linha 34 provavelmente está aqui ou em uma função chamada por ela
            if (olharParaJogador)
            {
                RotacionarParaJogador();
            }
        }
        else
        {
            rb.velocity = Vector2.zero; // Para de se mover se não estiver seguindo
        }
    }

    void FixedUpdate()
    {
        if (jogoAcabou) return; // Se o jogo acabou, não aplica mais física de movimento

        // O movimento baseado em velocidade deve ser aplicado no FixedUpdate para consistência física
        if (seguindo)
        {
            // A lógica de movimento já está sendo chamada no Update dentro da condição 'seguindo'
        }
    }

    void SeguirGatinhoDireto()
    {
        if (jogoAcabou) return; // Se o jogo acabou, não segue mais

        if (personagem != null)
        {
            float distanciaAoJogador = Vector2.Distance(transform.position, personagem.position);

            if (distanciaAoJogador > distanciaMinima)
            {
                Vector2 direcao = (personagem.position - transform.position).normalized;
                rb.velocity = direcao * velocidadeAranha; // Aqui 'rb' é usado
            }
            else
            {
                rb.velocity = Vector2.zero; // Parar de se mover quando estiver perto o suficiente
            }
        }
    }

    void RotacionarParaJogador()
    {
        if (jogoAcabou) return; // Se o jogo acabou, não rotaciona mais

        if (personagem != null)
        {
            Vector2 direcao = personagem.position - transform.position;
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            Quaternion rotacaoAlvo = Quaternion.AngleAxis(angulo - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, rotacaoVelocidade * Time.deltaTime);
        }
    }

    IEnumerator AutodestruirAposTempo()
    {
        yield return new WaitForSeconds(tempoDeVidaPerseguindo);
        Morrer();
    }

    void Morrer()
    {
        if (jogoAcabou) return; // Se o jogo acabou, não executa a lógica de morte normal

        // Lógica de morte da aranha
        Debug.Log("Aranha morreu!");
        if (efeitoMorte != null)
        {
            Instantiate(efeitoMorte, transform.position, Quaternion.identity);
        }
        // Se o som da aranha estiver tocando, para
        if (audioSourceAranha != null && audioSourceAranha.isPlaying)
        {
            audioSourceAranha.Stop();
        }
        Destroy(gameObject); // Destrói o GameObject da aranha
    }

    void TocarSomIniciarPerseguicao()
    {
        if (!jogoAcabou && audioSourceAranha != null && somIniciarPerseguicao != null)
        {
            audioSourceAranha.PlayOneShot(somIniciarPerseguicao);
        }
    }

    IEnumerator FadeOutAudioAfterDelay()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        if (!jogoAcabou)
        {
            yield return StartCoroutine(FadeOutAudio(audioSourceAranha, fadeOutDuration));
        }
    }

    IEnumerator FadeOutAudio(AudioSource audioSource, float duration)
    {
        float time = 0;
        float startVolume = audioSource.volume;
        float endVolume = 0;

        while (time < duration && !jogoAcabou)
        {
            time += Time.deltaTime * Time.timeScale; // Considera a escala de tempo
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, time / duration);
            yield return null;
        }

        audioSource.volume = endVolume; // Garante que o volume final seja zero
        if (jogoAcabou && audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Garante que o som pare se o jogo acabar durante o fade out
        }
    }

    void PararAranha()
    {
        jogoAcabou = true;
        // Se o som da aranha estiver tocando, para imediatamente
        if (audioSourceAranha != null && audioSourceAranha.isPlaying)
        {
            audioSourceAranha.Stop();
        }
        StopAllCoroutines(); // Para todas as corrotinas neste GameObject
        // O movimento da aranha já será parado porque a lógica de movimento verifica 'jogoAcabou' e Time.timeScale = 0f
    }
}