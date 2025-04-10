using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float flutuacaoVelocidade = 1f;
    public float flutuacaoAltura = 0.2f;
    public float rotacaoVelocidade = 50f;
    public float tempoDeVida = 6f;

    private Vector3 posicaoInicial;
    private float tempoCriacao;

    void Start()
    {
        posicaoInicial = transform.position;
        tempoCriacao = Time.time;
    }

    void Update()
    {
        // Movimento de Flutuação (Senoide)
        float novoY = posicaoInicial.y + Mathf.Sin((Time.time - tempoCriacao) * flutuacaoVelocidade) * flutuacaoAltura;
        transform.position = new Vector3(transform.position.x, novoY, transform.position.z);

        // Rotação em Y
        transform.Rotate(Vector3.up * rotacaoVelocidade * Time.deltaTime);

        // Destruição após tempo de vida
        if (Time.time - tempoCriacao >= tempoDeVida)
        {
            Destroy(gameObject);
        }
    }

    // Chamado quando outro collider entra no trigger deste power-up
    void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que colidiu tem a tag de um dos jogadores
        if (other.CompareTag("PlayerPrincipal") || other.CompareTag("PlayerSecundario"))
        {
            // Encontra o script de controle do robô no objeto que colidiu
            RoboControler principal = other.GetComponent<RoboControler>();
            if (principal != null)
            {
                principal.powerUpAtivo = true;
                principal.tempoPowerUpAtivo = 0f;
                Destroy(gameObject);
                return;
            }

            Robo1Controler secundario = other.GetComponent<Robo1Controler>();
            if (secundario != null)
            {
                secundario.powerUpAtivo = true;
                secundario.tempoPowerUpAtivo = 0f;
                Destroy(gameObject);
                return;
            }
        }
    }
}