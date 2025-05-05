using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboControler : MonoBehaviour
{
    // *** Variáveis Públicas (Configuráveis no Inspector da Unity) ***
    public float velocidadeMaxima = 5f;
    public float aceleracao = 2f;
    public float velocidadeRotacao = 100f;
    public float passoParaTrasDistancia = 1f;
    public float tempoVelocidadeMaxima = 3f;
    public float intervaloDesaceleracao = 0.5f;
    public float taxaDesaceleracao = 0.2f;
    public Transform cabeca;
    public float alturaAbaixado = 0.5f;
    public float tempoMaxAbaixado = 2f; // Tempo máximo que a cabeça pode ficar abaixada
    public float tempoPowerUpAtivo = 0f;
    public bool powerUpAtivo = false;

    // *** Variáveis Privadas (Uso Interno do Script) ***
    private float velocidadeAtual = 0f;
    private bool acelerando = false;
    private float tempoEmVelocidadeMaxima = 0f;
    private float ultimoTempoDesaceleracao = 0f;
    private bool podeDesacelerar = false;
    private bool abaixado = false;
    private float tempoAbaixadoAtual = 0f; // Tempo atual que a cabeça está abaixada
    private Vector3 posicaoInicialCabeca;
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

        if (cabeca == null)
        {
            Debug.LogError("A referência ao objeto da 'cabeça' não foi definida no Inspector!");
            enabled = false;
            return;
        }

        posicaoInicialCabeca = cabeca.localPosition;
    }

    void Update()
    {
        // *** Movimentação e Aceleração ***
        if (Input.GetKey(KeyCode.W))
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
            // Parar a aceleração quando a tecla não está pressionada
            if (!Input.GetKey(KeyCode.W))
            {
                // Se não estiver mais acelerando e a velocidade atual for maior que zero,
                // podemos adicionar uma desaceleração natural aqui se desejado.
                // Por enquanto, vamos apenas parar a velocidade imediatamente.
                if (!powerUpAtivo) // Impede o reset da velocidade durante o power-up
                {
                    velocidadeAtual = Mathf.Max(0f, velocidadeAtual - aceleracao * Time.deltaTime); // Desaceleração gradual
                    if (!Input.GetKey(KeyCode.W) && velocidadeAtual < 0.01f) velocidadeAtual = 0f; // Para completamente
                }
            }
        }

        // *** Rotação ***
        float rotacao = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            rotacao -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotacao += 1f;
        }
        // Aplica a rotação baseada na entrada
        transform.Rotate(Vector3.up * rotacao * velocidadeRotacao * Time.deltaTime);

        // *** Passo para Trás ***
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 direcaoOposta = -transform.forward;
            rb.MovePosition(rb.position + direcaoOposta * passoParaTrasDistancia);
            velocidadeAtual = 0f;
            acelerando = false;
            tempoEmVelocidadeMaxima = 0f;
            podeDesacelerar = false;
        }

        // *** Abaixar a Cabeça ***
        if (Input.GetKey(KeyCode.Space))
        {
            tempoAbaixadoAtual += Time.deltaTime;
            if (tempoAbaixadoAtual <= tempoMaxAbaixado)
            {
                abaixado = true;
                cabeca.localPosition = new Vector3(cabeca.localPosition.x, alturaAbaixado, cabeca.localPosition.z);
            }
            else
            {
                abaixado = false;
                cabeca.localPosition = new Vector3(cabeca.localPosition.x, posicaoInicialCabeca.y, cabeca.localPosition.z);
            }
        }
        else
        {
            abaixado = false;
            tempoAbaixadoAtual = 0f; // Reseta o contador quando a tecla é solta
            cabeca.localPosition = new Vector3(cabeca.localPosition.x, posicaoInicialCabeca.y, cabeca.localPosition.z);
        }

        // *** Aplicar Movimento ***
        Vector3 movimento = transform.forward * velocidadeAtual * Time.deltaTime;
        rb.MovePosition(rb.position + movimento);

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