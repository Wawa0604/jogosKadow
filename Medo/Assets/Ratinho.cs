using UnityEngine;

public class Ratinho : MonoBehaviour
{
    public float velocidadeFuga = 5f;
    public float distanciaSeguranca = 4f; // Distância mínima que o rato tenta manter do gato
    
    public LayerMask layerJogador; // Camada do GameObject do jogador (gato)
    private Rigidbody2D rb;
    private Transform jogadorTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Encontra o Transform do jogador pela tag (ajuste a tag se necessário)
        GameObject jogador = GameObject.FindGameObjectWithTag("Player");
        if (jogador != null)
        {
            jogadorTransform = jogador.transform;
        }
        else
        {
            Debug.LogError("Jogador não encontrado com a tag 'Player'!");
            enabled = false; // Desativa o script se o jogador não for encontrado
        }
    }

    void FixedUpdate() // Usando FixedUpdate para movimentos baseados em física
    {
        if (jogadorTransform != null)
        {
            float distanciaAoJogador = Vector2.Distance(transform.position, jogadorTransform.position);

            if (distanciaAoJogador < distanciaSeguranca)
            {
                // Calcula a direção de fuga (do rato para longe do jogador)
                Vector2 direcaoFuga = (transform.position - jogadorTransform.position).normalized;
                // Aplica uma força para se afastar
                rb.AddForce(direcaoFuga * velocidadeFuga, ForceMode2D.Force);
            }
            else
            {
                // Se o jogador estiver longe, o rato pode ficar parado ou ter outro comportamento
                rb.velocity = Vector2.zero; // Por enquanto, apenas para o movimento
            }
        }
    }

    // Para visualização no Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanciaSeguranca);
    }
}