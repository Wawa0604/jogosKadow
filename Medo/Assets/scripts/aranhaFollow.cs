using UnityEngine;
using System.Collections; // Necessário para usar Coroutines

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

    private Rigidbody2D rb;
    private bool seguindo = false;
    private float tempoInicioPerseguicao;

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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            colidindoInicial = true; // O jogador entrou no raio de ativação
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            colidindoInicial = false;
        }
    }

    void Update()
    {
        // Se o jogador estiver (ou esteve) dentro do raio inicial e pressionar Espaço, inicie a perseguição
        if (colidindoInicial && Input.GetKeyDown(KeyCode.Space) && !seguindo)
        {
            seguindo = true;
            tempoInicioPerseguicao = Time.time; // Registra o tempo de início da perseguição
            StartCoroutine(AutodestruirAposTempo());
        }

        // Se estiver seguindo, move e rotaciona
        if (seguindo)
        {
            SeguirGatinhoDireto();
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
        // O movimento baseado em velocidade deve ser aplicado no FixedUpdate para consistência física
        if (seguindo)
        {
            // A lógica de movimento já está sendo chamada no Update dentro da condição 'seguindo'
        }
    }

    void SeguirGatinhoDireto()
    {
        if (personagem != null)
        {
            float distanciaAoJogador = Vector2.Distance(transform.position, personagem.position);

            if (distanciaAoJogador > distanciaMinima)
            {
                Vector2 direcao = (personagem.position - transform.position).normalized;
                rb.velocity = direcao * velocidadeAranha;
            }
            else
            {
                rb.velocity = Vector2.zero; // Parar de se mover quando estiver perto o suficiente
            }
        }
    }

    void RotacionarParaJogador()
    {
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
        // Lógica de morte da aranha
        Debug.Log("Aranha morreu!");
        if (efeitoMorte != null)
        {
            Instantiate(efeitoMorte, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // Destrói o GameObject da aranha
    }
}