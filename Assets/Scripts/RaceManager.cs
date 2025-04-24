using UnityEngine;
using TMPro;
using System.Linq;
using System; // Se estiver usando TextMeshPro

public class RaceManager : MonoBehaviour
{
    public int voltasParaVencer = 3;
    public Transform[] checkpoints; // Arraste os GameObjects dos checkpoints aqui na ordem

    [Header("UI Text")]
    public TextMeshProUGUI voltasPlayerPrincipalTexto; // Arraste o Text UI do Player Principal aqui
    public TextMeshProUGUI voltasPlayerSecundarioTexto; // Arraste o Text UI do Player Secundario aqui

    private int voltasPlayerPrincipal = 0;
    private int checkpointAtualPlayerPrincipal = 0;

    private int voltasPlayerSecundario = 0;
    private int checkpointAtualPlayerSecundario = 0;

    private bool jogoAcabou = false;

    void Start()
    {
        // Verificação mais robusta para garantir que os componentes necessários estão atribuídos
        if (checkpoints == null || checkpoints.Length < 1 || voltasPlayerPrincipalTexto == null || voltasPlayerSecundarioTexto == null)
        {
            Debug.LogError("Por favor, configure corretamente os checkpoints e os textos da UI no RaceManager!");
            enabled = false;
            return; // Importante sair do Start se a configuração for inválida
        }

        AtualizarUITexto();
    }

    void OnTriggerEnter(Collider other)
    {
        if (jogoAcabou) return;

        if (other.CompareTag("PlayerPrincipal"))
        {
            ProcessarCheckpoint(other.transform, ref checkpointAtualPlayerPrincipal, ref voltasPlayerPrincipal, voltasPlayerPrincipalTexto, "Player Principal");
        }
        else if (other.CompareTag("PlayerSecundario"))
        {
            ProcessarCheckpoint(other.transform, ref checkpointAtualPlayerSecundario, ref voltasPlayerSecundario, voltasPlayerSecundarioTexto, "Player Secundário");
        }
    }

    void ProcessarCheckpoint(Transform playerTransform, ref int checkpointAtual, ref int voltas, TextMeshProUGUI textoUI, string nomePlayer)
    {
        // Verificou se entrou no checkpoint esperado
        if (playerTransform == checkpoints[checkpointAtual])
        {
            Debug.Log($"{nomePlayer} passou pelo Checkpoint {checkpointAtual + 1}");
            checkpointAtual++;

            // Se passou por todos os checkpoints, completa uma volta
            if (checkpointAtual >= checkpoints.Length)
            {
                voltas++;
                checkpointAtual = 0;
                AtualizarUITexto();
                VerificarFimDeJogo();
                Debug.Log($"{nomePlayer} completou a volta {voltas}");
            }
        }
        else
        {
            // Se passou por um checkpoint fora de ordem
            int checkpointIndex = Array.IndexOf(checkpoints, playerTransform);
            if (checkpointIndex != -1)
            {
                if (checkpointIndex < checkpointAtual)
                {
                    Debug.Log($"{nomePlayer} passou pelo Checkpoint {checkpointIndex + 1} novamente.");
                }
                else if (checkpointIndex > checkpointAtual)
                {
                    Debug.Log($"{nomePlayer} pulou para o Checkpoint {checkpointIndex + 1}. Volte para o próximo checkpoint.");
                }
            }
        }
    }

    private void VerificarFimDeJogo()
    {
        if (!jogoAcabou)
        {
            if (voltasPlayerPrincipal >= voltasParaVencer)
            {
                Debug.Log("Player Principal venceu a corrida!");
                jogoAcabou = true;
                // Adicionar lógica para exibir tela de vitória do Player Principal
            }
            else if (voltasPlayerSecundario >= voltasParaVencer)
            {
                Debug.Log("Player Secundário venceu a corrida!");
                jogoAcabou = true;
                // Adicionar lógica para exibir tela de vitória do Player Secundário
            }
        }
    }

    private void AtualizarUITexto()
    {
        if (voltasPlayerPrincipalTexto != null)
        {
            voltasPlayerPrincipalTexto.text = "Voltas: " + voltasPlayerPrincipal + "/" + voltasParaVencer;
        }
        if (voltasPlayerSecundarioTexto != null)
        {
            voltasPlayerSecundarioTexto.text = "Voltas: " + voltasPlayerSecundario + "/" + voltasParaVencer;
        }
    }
}