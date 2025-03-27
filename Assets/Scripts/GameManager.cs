using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Manages the overall game flow, including board setup, player interactions,
/// AI moves, game status updates, and UI handling for the solitaire game.
/// </summary>
public class GameManager : MonoBehaviour 
{
    ///<summary>
    /// Defines the different board types available in the game.
    /// </summary>
    private enum BoardType
    {
        French,
        German,
        Asymmetrical,
        English,
        Diamond,
        Triangular
    }
    
    /// <summary>
    /// Defines the two possible game modes: User or Computer.
    /// </summary>
    private enum GameMode
    {
        User,
        Computer
    }

    [FormerlySerializedAs("boardDB")] [SerializeField] private BoardLayoutDataBase boardLayoutDB;
    [SerializeField] private Image gameOverPanel;
    [SerializeField] private GameObject gameRulesPanel;
    [SerializeField] private Cell cellPrefab; 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip wrongMoveSound;
    [SerializeField] private TextMeshProUGUI textNumberOfMoves;
    [SerializeField] private TextMeshProUGUI textNumberOfPegs;
    [SerializeField] private Button undoButton; 
    [SerializeField] private Button nextMoveButton; 
    
    private GameMode _gameMode;
    private BoardType _gameBoardType; 
    private int _boardWidth; 
    private int _boardHeight;
    private Dictionary<Vector2Int, Cell> _cells;
    private Stack<Movement> _allMovements;
    private List<Cell> _possibleEndCells;
    private Cell _startCell;
    private int _numberOfMoves; 
    private int _numberOfPegs; 

    void Awake() 
    {
        _cells = new Dictionary<Vector2Int, Cell>();
        _startCell = null;
        _allMovements = new Stack<Movement>();
        _possibleEndCells = new List<Cell>();
        
        _gameMode = (PlayerPrefs.HasKey("_selectedGameModeOption")) ?
            (GameMode) PlayerPrefs.GetInt("_selectedGameModeOption") : 0;
        _gameBoardType = (PlayerPrefs.HasKey("_selectedBoardOption")) ? 
            (BoardType) PlayerPrefs.GetInt("_selectedBoardOption") : 0;
    }

    void Start() 
    {
        LoadGame();
        mainCamera.transform.position = new Vector3((float) _boardWidth / 2 - 0.5f, (float) _boardHeight / 2 , -10);
        undoButton.gameObject.SetActive(false);
        
        // AI Gaming
        if (_gameMode == GameMode.Computer) 
        {
            nextMoveButton.gameObject.SetActive(false);
            StartCoroutine(PlayAuto());
        }
    }

    void Update() 
    {
        if (_gameMode == GameMode.User && !gameRulesPanel.gameObject.activeSelf)
            GetSelection();
    }   
    
    /// <summary>
    /// Handles cell selection logic based on user input.
    /// </summary>
    private void GetSelection()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo)) 
            {
                Cell selectedCell = hitInfo.collider.gameObject.GetComponent<Cell>();
                if (selectedCell is not null) 
                {
                    if (selectedCell.GetValue() == Cell.CellValue.Peg) 
                    {
                        if (_startCell is not null) 
                        {
                            _startCell.SetValue(Cell.CellValue.Peg);
                            DisplayPossibleMove(false);
                        }
                        
                        _startCell = selectedCell;
                        _startCell.SetValue(Cell.CellValue.Selected);
                        DisplayPossibleMove(true);
                    }
                   
                    else if ((  selectedCell.GetValue() == Cell.CellValue.Empty || 
                                selectedCell.GetValue() == Cell.CellValue.Predicted) && 
                                _startCell is not null) 
                    { 
                        if (! MakeMove(_startCell, selectedCell))
                            _startCell.SetValue(Cell.CellValue.Peg);
                        
                        DisplayPossibleMove(false);
                        _startCell = null;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Displays or hides possible move predictions from the selected cell.
    /// </summary>
    /// <param name="isDisplayOn">True to show predictions, false to hide them.</param>
    private void DisplayPossibleMove(bool isDisplayOn) 
    {
        if (isDisplayOn) 
        {
            _possibleEndCells = GetPossibleEndPos(_startCell);
            if (_possibleEndCells is not null)
                foreach (var cell in _possibleEndCells)
                    cell.SetValue(Cell.CellValue.Predicted);
        }
        else if (_possibleEndCells is not null) 
        {
            Cell endCell = (_allMovements.Count > 0) ? _allMovements.Peek().GetEnd() : null;
            foreach (var cell in _possibleEndCells)
                if (cell != endCell) 
                    cell.SetValue(Cell.CellValue.Empty);
        }
    }
    
    
    /// <summary>
    /// Coroutine to let the AI play moves automatically.
    /// </summary>
    private IEnumerator PlayAuto() 
    {
        while (! IsGameOver()) 
        {
            yield return new WaitForSeconds(0.2f);
            MakeRandomMove();
            yield return new WaitForSeconds(1f);            
        }
    } 
    
    /// <summary>
    /// Coroutine to visually demonstrate an automated move.
    /// </summary>
    private IEnumerator AutoMove(Cell start, Cell end)
    {
        if (_gameMode == GameMode.User)
            nextMoveButton.gameObject.SetActive(false);
        start.SetValue(Cell.CellValue.Selected);
        yield return new WaitForSeconds(0.5f);            
        end.SetValue(Cell.CellValue.Predicted);
        yield return new WaitForSeconds(0.5f);            
        MakeMove(start, end); 
        if (_gameMode == GameMode.User)
            nextMoveButton.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Executes a move from the start cell to the end cell, updating game state if valid.
    /// </summary>
    /// <returns>True if the move is valid, false otherwise.</returns>
    private bool MakeMove(Cell start, Cell end) 
    {
        Movement mov = new Movement(_cells, start, end);
        if (mov.IsValidMovement()) 
        {
            Cell jump = mov.GetJump();
            
            start.SetValue(Cell.CellValue.Empty);
            jump.SetValue(Cell.CellValue.Empty);
            end.SetValue(Cell.CellValue.Peg);

            
            _numberOfMoves++;
            _numberOfPegs--;
            _allMovements.Push(mov);
            UpdateGameStatus();
            
            if (_gameMode == GameMode.User && !undoButton.gameObject.activeSelf)
                undoButton.gameObject.SetActive(true);
            
            if (IsGameOver())
                StartCoroutine(DisplayGameOverPanel());
            
            return true;
        }
        audioSource.PlayOneShot(wrongMoveSound, 0.1f);
        return false;
    }
    
    /// <summary>
    /// Selects a random starting cell and attempts to make a valid move.
    /// </summary>
    public void MakeRandomMove() 
    {
        int x = Random.Range(0, _boardWidth);
        int y = Random.Range(0, _boardHeight);
        
        List<Cell> endPos = null; 
        for (int i = 0; i < _boardWidth && endPos == null; i++) 
        {
            for (int j = 0; ++j < _boardHeight && endPos == null; j++) 
            {
                if (_cells.TryGetValue(new Vector2Int(x, y), out Cell start)) {
                    endPos = GetPossibleEndPos(start);
                    if (endPos != null) 
                    {
                        Cell end = endPos[Random.Range(0, endPos.Count)];
                        
                        StartCoroutine(AutoMove(start, end));
                    }
                }
                y = (y + 1 < _boardHeight) ? y + 1 : 0; 
            }
            x = (x + 1 < _boardWidth) ? x + 1 : 0; 
        }
    }
    
    /// <summary>
    /// Reverts the last move made in the game.
    /// </summary>
    public void Undo() 
    {
        if (_allMovements.Count > 0) 
        {
            var mov = _allMovements.Pop();
            mov.GetStart().SetValue(Cell.CellValue.Peg);
            mov.GetJump().SetValue(Cell.CellValue.Peg);
            mov.GetEnd().SetValue(Cell.CellValue.Empty);
            
            _numberOfMoves--;
            _numberOfPegs++;

            if (_allMovements.Count == 0)
                undoButton.gameObject.SetActive(false);

            UpdateGameStatus();
        }
    }
    
    /// <summary>
    /// Retrieves all possible end positions for a given start cell.
    /// </summary>
    private List<Cell> GetPossibleEndPos(Cell start) 
    {
        List<Cell> endPosition = new List<Cell>();
        if (start is not null && (start.GetValue() == Cell.CellValue.Peg || start.GetValue() == Cell.CellValue.Selected)) 
        {
            Movement mov = new Movement(_cells, start);
            for (int dir = 0; dir < 4; dir++) 
            {
                mov.SetMovement(start, (Movement.Direction) dir);
                if (mov.IsValidMovement())
                    endPosition.Add(mov.GetEnd());
            }
        }
        return endPosition.Count > 0 ? endPosition : null; 
    }
    
    /// <summary>
    /// Determines if there are no more valid moves, indicating the game is over.
    /// </summary>
    private bool IsGameOver() 
    {
        foreach (var cell in _cells.Values) 
            if (GetPossibleEndPos(cell) != null)
                return false;
        
        return true;
    }
    
    /// <summary>
    /// Loads the board layout from the database and initializes the game board.
    /// </summary>
    private void LoadGame()
    {
        List<string> lines = boardLayoutDB.GetBoardLayout((int) _gameBoardType);
        
        if (lines.Count > 0) 
        {
            string[] values = lines[0].Split();
            if (values.Length < 3) 
                throw new System.ArgumentException();

            _boardWidth = int.Parse(values[0]);
            _boardHeight = int.Parse(values[1]);
            _numberOfMoves = int.Parse(values[2]);
            _numberOfPegs = 0;

            Vector2Int pos = new Vector2Int(); 
            
            for (int y = lines.Count - 1; y > 0; y--) 
            {
                pos.y = _boardHeight - y;
                for (int x = 0; x <= lines[y].Length / 2; x++) 
                {
                    pos.x = x;
                    switch (lines[y][x * 2]) 
                    { 
                        case '.': // Empty
                            InstantiateCell(pos, Cell.CellValue.Empty); 
                            break;
                        
                        case 'P': // Peg
                            InstantiateCell(pos, Cell.CellValue.Peg); 
                            _numberOfPegs++;
                            break;
                    }
                }
            }
        }  
    }
    
    /// <summary>
    /// Returns the player to the main menu, saving scores if in User mode.
    /// </summary>
    public void ReturnMainMenu() 
    {
        _gameMode = (PlayerPrefs.HasKey("_selectedGameModeOption")) ?
            (GameMode) PlayerPrefs.GetInt("_selectedGameModeOption") : 0;
        
        if (_gameMode == GameMode.User)
            PlayerPrefs.SetInt("_" + (int) _gameBoardType + "BoardScore", _numberOfPegs);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
    /// <summary>
    /// Displays the game over panel with the appropriate sound based on the game's outcome.
    /// </summary>
    private IEnumerator DisplayGameOverPanel() 
    {
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(_numberOfPegs == 1 ? winSound : loseSound);
        gameOverPanel.gameObject.SetActive(true);
    }

    private void UpdateGameStatus() {
       textNumberOfMoves.text = $"Number Of Movement:  {_numberOfMoves.ToString()}";
       textNumberOfPegs.text = $"Peg Remain:  {_numberOfPegs.ToString()}";
    }
    
    /// <summary>
    /// Reloads the current scene to restart the game.
    /// </summary>
    public void ReplayGame() 
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
    /// <summary>
    /// Instantiates a cell at the specified position with the given value.
    /// </summary>
    private void InstantiateCell(Vector2Int pos, Cell.CellValue val) 
    {
        Cell cell = Instantiate(cellPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        cell.Initialize(pos, val);
        _cells.Add(pos, cell);
    }
}
