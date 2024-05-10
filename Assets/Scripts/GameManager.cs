using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text countdownText; // Referencia al texto de la cuenta regresiva en la interfaz de usuario
    public float startDuration = 1f; // Duración de la visualización de "START"

    public Board board;

    private bool isPaused = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        board = GetComponent<Board>();
        // Comienza el juego después de la duración de inicio
        StartCoroutine(StartGame());
    }
    private void Update()
    {
        // Verifica si se ha presionado la tecla P para pausar o reanudar el juego
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    private IEnumerator StartGame()
    {
        // Mostrar "START" durante un breve período de tiempo antes de comenzar el juego
        countdownText.text = "START";
        yield return new WaitForSeconds(startDuration);

        // Llama al método para iniciar el juego en el componente Board
        if (board != null)
        {
            board.Start(); // Llama al método Start en el componente Board
        }

        // Una vez que se ha mostrado "START" el tiempo suficiente, oculta el texto
        countdownText.enabled = false;
    }
    private void TogglePause()
    {
        // Cambia el estado de pausa
        isPaused = !isPaused;

        // Si el juego está pausado, detén el tiempo del juego
        if (isPaused)
        {
            audioSource.Pause();
            Time.timeScale = 0f;
        }
        else // Si el juego está reanudado, restablece el tiempo del juego a su velocidad normal
        {
            audioSource.UnPause();
            Time.timeScale = 1f;
        }
    }
}
