using UnityEngine; 
using TMPro; 
using System.Collections;
using System; // Importa o namespace System.Collections, que permite usar interfaces como IEnumerator para corrotinas.

public class LightSwitch : MonoBehaviour 
{
    private bool colidindo = false; 
    private bool aceso = false; 
    private bool interagindoComAbajur = false; 
    public TextMeshProUGUI meuTexto; // Declara uma variável pública 'meuTexto' do tipo TextMeshProUGUI. Este campo será usado para atribuir o componente de texto da interface do usuário no Inspector da Unity.
    public GameObject luzAbajur; // Declara uma variável pública 'luzAbajur' do tipo GameObject. Este campo será usado para atribuir o GameObject que representa a luz do abajur no Inspector da Unity.
    
    void Start() // Função Start é chamada uma vez quando o script é carregado.
    {
        meuTexto = GameObject.Find("meuTexto").GetComponent<TextMeshProUGUI>(); // Procura um GameObject na cena com o nome "meuTexto" e obtém seu componente TextMeshProUGUI, atribuindo-o à variável 'meuTexto'. É importante que um GameObject com esse nome e componente exista na cena.
        // Garante que a luz comece desligada (opcional)
        if (luzAbajur != null && !aceso) // Verifica se 'luzAbajur' foi atribuído no Inspector e se a luz não está acesa inicialmente.
        {
            luzAbajur.SetActive(false); // Desativa o GameObject da luz do abajur, garantindo que comece desligada.
        }
        AtualizarTexto(); // Chama a função 'AtualizarTexto' para definir o texto inicial da interface do usuário com base no estado inicial.
    }

    private void OnTriggerEnter2D(Collider2D other) // Função OnTriggerEnter2D é chamada quando outro collider 2D entra no trigger collider 2D anexado a este GameObject.
    {
        if (other.CompareTag("Player")) // Verifica se o GameObject que entrou no trigger tem a tag definida como "Player". A tag "Player" precisa ser atribuída ao GameObject do jogador no Inspector da Unity.
        {
            colidindo = true; // Define a variável 'colidindo' como verdadeira, indicando que o jogador está dentro da área de interação.
            AtualizarTexto(); // Chama a função 'AtualizarTexto' para atualizar o texto da interface do usuário para a ação de ligar/desligar.
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Função OnTriggerExit2D é chamada quando outro collider 2D sai do trigger collider 2D anexado a este GameObject.
    {
        if (other.CompareTag("Player")) // Verifica se o GameObject que saiu do trigger tinha a tag definida como "Player".
        {
            colidindo = false; // Define a variável 'colidindo' como falsa, indicando que o jogador saiu da área de interação.
            meuTexto.text = ""; // Limpa o texto da interface do usuário quando o jogador sai da área de interação.
        }
    }

    IEnumerator interageAbajur() // Declara uma corrotina chamada 'interageAbajur'. Corrotinas podem pausar sua execução e retomá-la posteriormente, útil para ações com duração.
    {
        interagindoComAbajur = true; // Define a variável 'interagindoComAbajur' como verdadeira para evitar que a interação seja iniciada novamente antes que a atual termine.

        if (luzAbajur != null) // Verifica se o GameObject 'luzAbajur' foi atribuído no Inspector.
        {
            aceso = !aceso; // Inverte o estado da variável 'aceso' (se era verdadeiro, torna-se falso, e vice-versa).
            luzAbajur.SetActive(aceso); // Ativa ou desativa o GameObject da luz do abajur com base no novo valor da variável 'aceso'.
            AtualizarTexto(); // Chama a função 'AtualizarTexto' para atualizar o texto da interface do usuário para a ação oposta (ligar se estava desligado, desligar se estava ligado).
        }
        else // Se 'luzAbajur' não foi atribuído.
        {
            Debug.LogWarning("O GameObject 'luzAbajur' não está atribuído ao interruptor!"); // Exibe um aviso no Console da Unity para alertar sobre a falta de atribuição.
        }

        yield return new WaitForSeconds(0.5f); // Pausa a execução desta corrotina por 0.5 segundos. Isso cria um pequeno atraso antes de permitir outra interação.
        interagindoComAbajur = false; // Define a variável 'interagindoComAbajur' como falsa novamente, permitindo futuras interações.
    }

    void Update() // Função Update é chamada a cada frame.
    {
        // Verifica se o jogador está colidindo com o trigger, se não está interagindo com o abajur no momento e se o botão definido no Input Manager com o nome em 'nomeAcao' (tecla Espaço) foi pressionado neste frame.
        if (colidindo && !interagindoComAbajur && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(interageAbajur()); // Inicia a execução da corrotina 'interageAbajur', que lida com a lógica de ligar/desligar a luz.
        }
        // if (Input.GetKeyDown(KeyCode.Space)){
        //     Debug.Log("roberto");
        // }
    }

    void AtualizarTexto() // Função para atualizar o texto da interface do usuário com base no estado da luz e da colisão com o jogador.
    {
        if (colidindo) // Verifica se o jogador está dentro da área de interação.
        {
            meuTexto.text = aceso ? "Desligar luz" : "Ligar luz"; // Se estiver colidindo, define o texto para "Desligar abajur" se a luz estiver acesa, ou "Ligar abajur" se estiver desligada.
        }
    }
}