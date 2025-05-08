using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GatinhoVida : MonoBehaviour

{
    public int vidas = 3;
    public TextMeshProUGUI meuTexto;
    public string textoInicial = "Vidas: ";

    // Evento para notificar quando uma vida é perdida
    public delegate void VidaPerdidaAction(int vidasRestantes);
    public static event VidaPerdidaAction OnVidaPerdida;

    void Start()
    {
        // Tenta encontrar o componente TextMeshProUGUI pelo nome
        meuTexto = GameObject.Find("VidaGatinho").GetComponent<TextMeshProUGUI>();

        // Se o componente de texto não for encontrado, exibe um aviso
        if (meuTexto == null)
        {
            Debug.LogWarning("Componente TextMeshProUGUI 'VidaGatinho' não encontrado na cena!");
        }

        // Atualiza o texto inicial das vidas
        AtualizarTexto();

        // Se inscreve no evento de perda de vida para atualizar o texto
        OnVidaPerdida += AtualizarTextoExterno;
    }

    // Atualiza o texto da interface do usuário com a contagem de vidas
    private void AtualizarTexto()
    {
        if (meuTexto != null)
        {
            meuTexto.text = textoInicial + vidas.ToString();
        }
    }

    // Função para ser chamada externamente quando uma vida é perdida
    private void AtualizarTextoExterno(int vidasRestantes)
    {
        vidas = vidasRestantes;
        AtualizarTexto();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Aranha"))
        {
            PerderVida();
        }
    }

    public void PerderVida()
    {
        vidas--;
        Debug.Log("Gatinho perdeu uma vida! Vidas restantes: " + vidas);

        // Notifica outros scripts sobre a perda de vida
        if (OnVidaPerdida != null)
        {
            OnVidaPerdida(vidas);
        }

        // Lógica adicional quando as vidas acabam (opcional)
        if (vidas <= 0)
        {
            Debug.Log("Gatinho morreu!");
            // Adicione aqui a lógica de game over ou respawn
            Destroy(gameObject); // Por enquanto, destrói o gatinho
        }
    }

    // Função para adicionar vidas (se necessário)
    public void AdicionarVida(int vidasGanhas)
    {
        vidas += vidasGanhas;
        Debug.Log("Gatinho ganhou " + vidasGanhas + " vida(s)! Vidas restantes: " + vidas);
        AtualizarTexto();
    }

    // Garante que o evento seja desinscrito quando o objeto é destruído
    private void OnDestroy()
    {
        OnVidaPerdida -= AtualizarTextoExterno;
    }
}