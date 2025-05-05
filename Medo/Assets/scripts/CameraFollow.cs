using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public Vector3 offset;   // Deslocamento entre a câmera e o jogador
    public float smoothSpeed = 0.125f; // Velocidade de suavização do movimento da câmera

    public float minX; // Limite mínimo no eixo X
    public float maxX; // Limite máximo no eixo X
    public float minY; // Limite mínimo no eixo Y (opcional)
    public float maxY; // Limite máximo no eixo Y (opcional)

    private float initialZ; // Armazena o valor Z inicial da câmera

    void Start()
    {
        // Captura o valor Z inicial da câmera para mantê-lo constante
        initialZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (player == null) return; // Evita erros se o jogador não estiver definido

        // Define a posição desejada da câmera com base na posição do jogador + o offset
        Vector3 desiredPosition = player.position + offset;

        // Limita os valores de X e Y da posição desejada
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY); // Se você tiver limites em Y

        // Move a câmera de forma suave para a posição limitada, mantendo o valor Z original
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(desiredPosition.x, desiredPosition.y, initialZ), smoothSpeed);

        // Atualiza a posição da câmera
        transform.position = smoothedPosition;
    }
}