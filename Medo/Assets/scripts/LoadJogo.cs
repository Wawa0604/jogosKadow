using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting; // This namespace is likely not needed for scene loading
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadJogo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Initialization code can go here if needed
    }

    // Update is called once per frame
    void Update()
    {
        // This will attempt to load scene with index 1 every frame while the mouse button is held down
        if (Input.GetMouseButtonDown(0)) // 0 represents the left mouse button
        {
            SceneManager.LoadScene(1);
        }
    }
}