using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robo1Controler : MonoBehaviour
{
    // *** Variáveis Públicas (Configuráveis no Inspector da Unity) ***
    public float velocidadeMaxima = 5f;
    public float aceleracao = 2f;
    public float velocidadeRotacao = 100f;
    public float passoParaTrasDistancia = 1f;
    public float tempoVelocidadeMaxima = 3f;
    public float intervaloDesaceleracao = 0.5f;
    public float taxaDesaceleracao = 0.2f;
    public Transform cabecaSecundario; // Referência ao objeto da "cabeça" do robô secundário
    public float alturaAbaixado = 0.5f; // A altura Y local da cabeça quando abaixada
    public float tempoPowerUpAtivo = 0f;
    public bool powerUpAtivo = false;

    // *** Variáveis Privadas (Uso Interno do Script) ***
    private float velocidadeAtual = 0f;
    private bool acelerando = false;
    private float tempoEmVelocidadeMaxima = 0f;
    private float ultimoTempoDesaceleracao = 0f;
    private bool podeDesacelerar = false;
    private bool abaixado = false;
    private Vector3 posicaoInicialCabeca; // Guarda a posição local inicial da cabeça
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("O objeto do robô precisa de um componente Rigidbody!");
            enabled = false;
            return;
        }

        // Garante que a referência à cabeça foi definida
        if (cabecaSecundario == null)
        {
            Debug.LogError("A referência ao objeto da 'cabeça secundário' não foi definida no Inspector!");
            enabled = false;
            return;
        }

        // Guarda a posição local inicial da cabeça
        posicaoInicialCabeca = cabecaSecundario.localPosition;
    }

    void Update()
    {
        // *** Movimentação e Aceleração ***
        if (Input.GetKey(KeyCode.UpArrow))
        {
            acelerando = true;
            if (velocidadeAtual < velocidadeMaxima)
            {
                velocidadeAtual += aceleracao * Time.deltaTime;
                velocidadeAtual = Mathf.Min(velocidadeAtual, velocidadeMaxima);
            }

            if (velocidadeAtual >= velocidadeMaxima)
            {
                tempoEmVelocidadeMaxima += Time.deltaTime;
                if (tempoEmVelocidadeMaxima >= tempoVelocidadeMaxima)
                {
                    podeDesacelerar = true;
                    if (Time.time - ultimoTempoDesaceleracao >= intervaloDesaceleracao)
                    {
                        velocidadeAtual -= velocidadeMaxima * taxaDesaceleracao;
                        velocidadeAtual = Mathf.Max(velocidadeAtual, velocidadeMaxima * 0.5f);
                        ultimoTempoDesaceleracao = Time.time;
                        if (velocidadeAtual <= velocidadeMaxima * 0.5f)
                        {
                            podeDesacelerar = false;
                        }
                    }
                }
            }
        }
        else
        {
            acelerando = false;
            tempoEmVelocidadeMaxima = 0f;
            podeDesacelerar = false;
            if (!Input.GetKey(KeyCode.UpArrow))
            {
                if (!powerUpAtivo)
                {
                    velocidadeAtual = Mathf.Max(0f, velocidadeAtual - aceleracao * Time.deltaTime);
                    if (!Input.GetKey(KeyCode.UpArrow) && velocidadeAtual < 0.01f) velocidadeAtual = 0f;
                }
            }
        }

        // *** Rotação ***
        float rotacao = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotacao -= 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotacao += 1f;
        }
        transform.Rotate(Vector3.up * rotacao * velocidadeRotacao * Time.deltaTime);

        // *** Passo para Trás ***
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 direcaoOposta = -transform.forward;
            rb.MovePosition(rb.position + direcaoOposta * passoParaTrasDistancia);
            velocidadeAtual = 0f;
            acelerando = false;
            tempoEmVelocidadeMaxima = 0f;
            podeDesacelerar = false;
        }

        // *** Abaixar a Cabeça ***
        if (Input.GetKey(KeyCode.RightControl)) // Tecla Ctrl direita
        {
            abaixado = true;
            // Move a cabeça para baixo no eixo Y (localmente ao pai)
            cabecaSecundario.localPosition = new Vector3(cabecaSecundario.localPosition.x, alturaAbaixado, cabecaSecundario.localPosition.z);
        }
        else
        {
            abaixado = false;
            // Retorna a cabeça para a sua posição Y local inicial
            cabecaSecundario.localPosition = new Vector3(cabecaSecundario.localPosition.x, posicaoInicialCabeca.y, cabecaSecundario.localPosition.z);
        }

        // *** Aplicar Movimento para Frente ***
        Vector3 movimentoParaFrente = transform.forward * velocidadeAtual * Time.deltaTime;
        rb.MovePosition(rb.position + movimentoParaFrente);

        // *** Lógica do Power-Up ***
        if (powerUpAtivo)
        {
            velocidadeAtual = velocidadeMaxima;
            acelerando = true;
            tempoPowerUpAtivo += Time.deltaTime;
            if (tempoPowerUpAtivo >= 4f)
            {
                powerUpAtivo = false;
                tempoPowerUpAtivo = 0f;
                tempoEmVelocidadeMaxima = tempoVelocidadeMaxima;
                podeDesacelerar = true;
                ultimoTempoDesaceleracao = Time.time;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstaculo"))
        {
            velocidadeAtual = velocidadeMaxima * 0.5f;
            acelerando = false;
            tempoEmVelocidadeMaxima = 0f;
            podeDesacelerar = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            powerUpAtivo = true;
            tempoPowerUpAtivo = 0f;
            Destroy(other.gameObject);
        }
    }
}