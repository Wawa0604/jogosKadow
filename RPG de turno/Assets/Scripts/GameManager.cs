using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Button cara;
    private Button coroa;
   

    // Start is called before the first frame update
    void Start()
    {
        cara = GameObject.Find("Cara").GetComponent<Button>();
        coroa = GameObject.Find("Coroa").GetComponent<Button>();
    
        cara.gameObject.SetActive(false);
        coroa.gameObject.SetActive(false);
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
