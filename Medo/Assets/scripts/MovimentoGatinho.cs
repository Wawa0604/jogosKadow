using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoGatinho : MonoBehaviour
{
    public float fatorDeMovimento;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Obtém o valor da entrada horizontal (-1, 0 ou 1)
        float movimentoHorizontal = Input.GetAxis("Horizontal");

        // Obtém o valor da entrada vertical (-1, 0 ou 1)
        float movimentoVertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(movimentoHorizontal, movimentoVertical, 0f);

        transform.position += movimento * fatorDeMovimento * Time.deltaTime;
    }
 
}
