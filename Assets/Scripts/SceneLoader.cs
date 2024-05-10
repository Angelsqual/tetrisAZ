using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Define la tecla que activará la transición de escena
    public KeyCode startKey = KeyCode.Return;

    // Este método se llama una vez por fotograma
    void Update()
    {
        // Verifica si se ha presionado la tecla definida para cargar la siguiente escena
        if (Input.GetKeyDown(startKey))
        {
            // Carga la siguiente escena en el índice siguiente al de la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
