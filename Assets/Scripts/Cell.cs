using UnityEngine;

/// <summary>
/// Represents a cell in the game grid. Handles cell position, value, and visual appearance.
/// </summary>
public class Cell : MonoBehaviour 
{
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Material predictedMaterial;
    [SerializeField] private Material pegMaterial;
    [SerializeField] private Material emptyMaterial;
    private Cell _up, _down, _left, _right;
    private Vector2Int _cellPosition; 
    private Renderer _renderer;
    private CellValue _cellValue;
    private bool _isHighlightOn;
    
    /// <summary>
    /// Enum representing possible cell states.
    /// </summary>
    public enum CellValue
    {
        Peg, 
        Empty, 
        Selected, 
        Predicted
    };
    
    
    // Start is called before the first frame update
    private void Awake() 
    {
        _renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Initializes the cell with a position and value.
    /// </summary>
    /// <param name="pos">Position of the cell in the grid.</param>
    /// <param name="value">Initial value of the cell.</param>
    public void Initialize(Vector2Int pos, CellValue value) 
    {
        name = pos.ToString();
        _cellPosition = pos;
        SetValue(value);  
    }

    /// <summary>
    /// Sets the position of the cell.
    /// </summary>
    /// <param name="pos">New position of the cell.</param>
    public void SetPosition(Vector2Int pos) 
        => _cellPosition = pos;
    
    public Vector2Int GetPosition() {return _cellPosition;}

    /// <summary>
    /// Sets the value of the cell and updates its material.
    /// </summary>
    /// <param name="value">New value of the cell.</param>
    public void SetValue(CellValue value) 
    {
        _cellValue = value;
        switch (value) 
        {
            case CellValue.Peg: 
                _renderer.material = pegMaterial; 
                break;
            
            case CellValue.Empty: 
                _renderer.material = emptyMaterial; 
                break;
            
            case CellValue.Selected: 
                _renderer.material = selectedMaterial; 
                break;
            
            case CellValue.Predicted: 
                _renderer.material = predictedMaterial; 
                break;
        }        
    }

    /// <summary>
    /// Retrieves the current value of the cell.
    /// </summary>
    /// <returns>The current cell value.</returns>
    public CellValue GetValue() 
            => _cellValue;

    /// <summary>
    /// Highlights the cell when the mouse enters, if the cell contains a Peg.
    /// </summary>
    private void OnMouseEnter() 
    {
        if (GetValue() == CellValue.Peg) 
        {
            _isHighlightOn = true;
            _renderer.material = selectedMaterial;
        }
    }
    
    /// <summary>
    /// Removes the highlight when the mouse exits, if the cell contains a Peg.
    /// </summary>
    private void OnMouseExit() 
    {
        if (_isHighlightOn && GetValue() == CellValue.Peg) 
        {
            _renderer.material = pegMaterial;
            _isHighlightOn = false;
        }
    }
    
    public Cell GetNeighbor(Movement.Direction direction, int distance = 1) 
    {
        return direction switch
        {
            Movement.Direction.Up => (distance == 1) ? _up : _up?._up,
            Movement.Direction.Down => (distance == 1) ? _down : _down?._down,
            Movement.Direction.Left => (distance == 1) ? _left : _left?._left,
            Movement.Direction.Right => (distance == 1) ? _right : _right?._right,
            _ => null
        };
    }

    public Cell GetAdjacentJumpCell(Cell end) 
    {
        if (end == _up?._up) return _up;
        if (end == _down?._down) return _down;
        if (end == _left?._left) return _left;
        if (end == _right?._right) return _right;
        return null;
    }
}