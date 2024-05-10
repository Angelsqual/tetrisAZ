using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);
    
    public int score=0;
    public int highScore { get; private set; }

    public AudioSource soundClearLine;
    public AudioSource soundGameOver;

    [Header("Speed Settings")]
    [SerializeField] private float speedIncrement = 0.1f; // Incremento de velocidad cada cierto número de líneas
    private int linesCleared = 0; // Contador de líneas borradas

    [Header("Score Settings")]
    [SerializeField] private int scorePerLine = 100; // Puntuación por línea borrada
    



    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        for (int i = 0; i < tetrominoes.Length; i++) {
            tetrominoes[i].Initialize();
        }
    }

    public void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition)) {
            Set(activePiece);
        } else {
            GameOver();
        }
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();
        soundGameOver.Play();

        score = 0;
        UIManager.instance.UpdateScoreText(score);
        UIManager.instance.UpdateHighScoreText(highScore);
 
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition)) {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;
        int linesClearedThisIteration = 0; // Contador para líneas borradas en esta iteración

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row)) {
                LineClear(row);
                linesClearedThisIteration++; // Incrementa el contador de líneas borradas
            } else {
                row++;
            }
        }
        // Actualizar el contador total de líneas borradas
        linesCleared += linesClearedThisIteration;

        // Incrementar la puntuación en función del número de líneas borradas
        score += linesClearedThisIteration * scorePerLine;

        // Actualiza la puntuación máxima si es necesario
        if (score > highScore) 
        {
        highScore = score;
        // Guarda el highScore actualizado en PlayerPrefs
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); // Guarda los cambios inmediatamente
        }

        // Actualiza la interfaz de usuario para mostrar la nueva puntuación
         UIManager.instance.UpdateScoreText(score);
         UIManager.instance.UpdateHighScoreText(highScore);

        // Verificar si se ha alcanzado el umbral de líneas para aumentar la velocidad
        if (linesCleared >= 5) // Puedes ajustar este valor según sea necesario
        {
            // Incrementar la velocidad del juego
            activePiece.stepDelay -= speedIncrement;
            linesCleared -= 5; // Reiniciar el contador de líneas borradas
        }
         
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position)) {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        soundClearLine.Play();

        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    
    }
    public void ResetScore()
    {
    score = 0;
    }
  

}