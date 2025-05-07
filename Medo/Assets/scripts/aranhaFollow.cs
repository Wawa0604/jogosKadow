using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aranhaFollow : MonoBehaviour
{
    public GameObject personagem;
    // Start is called before the first frame update
    void Start()
    {
        personagem = GameObject.Find("gatinho");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // IEnumerator aranhaSeguir()
    // {

    // }
}
