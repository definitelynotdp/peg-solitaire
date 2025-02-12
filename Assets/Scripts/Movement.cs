using System.Collections.Generic;
using UnityEngine;

public class Movement 
{
    private readonly Dictionary<Vector2Int, Cell> _board; // game board for checking validty of movement
    private (int, int) _dimension; // board dimension (number of rows and columns)
    private Cell _start; // start position of movement
    private Cell _jump; // jump position of movement (between start and end)
    private Cell _end; // end position of movement

    public enum Direction
    {
        Up, 
        Down, 
        Left, 
        Right
    };

    public Movement(Dictionary<Vector2Int, Cell> board, Cell start, Cell end) 
    {
        _board = board;
        try 
        {
            SetStart(start);
            SetEnd(end);
        }
        catch (System.ArgumentException e) 
        {
            System.Console.WriteLine($"Invalid arguments: {e}");
        }
    }

    public Movement(Dictionary<Vector2Int, Cell> board, Cell start) : this(board, start, null) {}

    public Movement(Dictionary<Vector2Int, Cell> board) : this(board, null, null) {}
    
    public Movement() : this(null, null, null) {}

    public Cell GetStart() => _start;
    
    public Cell GetJump() => _jump;
    
    public Cell GetEnd() => _end;

    private void SetStart(Cell start) 
    {
        if (_board.ContainsValue(start))
            _start = start;
        
        else 
        {
            _start = null;
            throw new System.ArgumentException("Given cell does not exist in game board");
        }
    }
    
    private void SetJump() 
    {
        if (_board is null || _start is null || _end is null) 
            _jump = null;
        
        else 
        {
            var startPos = _start.GetPosition();
            var endPos = _end.GetPosition();
            int jumpx = -1, jumpy = -1;
            
            // vertical movement
            if (startPos.x == endPos.x) 
            {
                jumpx = startPos.x;
                
                int diff = endPos.y - startPos.y;
                
                if (diff == 2) // up movement
                    jumpy = endPos.y - 1;
                
                else if (diff == -2) // down movement 
                    jumpy = endPos.y + 1;
            } 
            
            // horizontal movement  
            else if (startPos.y == endPos.y) 
            {
                jumpy = startPos.y;

                int diff = endPos.x - startPos.x;
                
                if (diff == 2) // right movement  
                    jumpx = endPos.x - 1;
                
                else if (diff == -2) // left movement
                    jumpx = endPos.x + 1; 
            }
            
            _jump = (_board.TryGetValue(new Vector2Int(jumpx, jumpy), out var jumpCell) && jumpCell.GetValue() == Cell.CellValue.Peg) ?
                        jumpCell : null;
        }
    }

    private void SetEnd(Cell end) 
    {
        if (_board.ContainsValue(end)) 
            _end = end;
        
        else 
        {
            _end = null;
            throw new System.ArgumentException("Given cell does not exist in game board");
        }
    }

    public void SetMovement(Cell start, Direction direction) 
    {
        try 
        {
            SetStart(start);
            Vector2Int startPos = start.GetPosition();

            switch (direction) 
            {
                case Direction.Up:
                    SetEnd(GetEndUp(startPos)); 
                    break;
                
                case Direction.Down: 
                    SetEnd(GetEndDown(startPos)); 
                    break;
                
                case Direction.Left: 
                    SetEnd(GetEndLeft(startPos)); 
                    break;
                
                case Direction.Right: 
                    SetEnd(GetEndRight(startPos)); 
                    break;
            }
            SetJump();
        }
        catch (System.ArgumentException) 
        {
            Debug.Log($"Invalid direction: {direction}");
        }
    }

    private Cell GetEndUp(Vector2Int startPos)
        => (_board.GetValueOrDefault(new Vector2Int(startPos.x, startPos.y + 2), null));

    private Cell GetEndDown(Vector2Int startPos)
        => (_board.GetValueOrDefault(new Vector2Int(startPos.x, startPos.y - 2), null));

    private Cell GetEndLeft(Vector2Int startPos)
        => (_board.GetValueOrDefault(new Vector2Int(startPos.x - 2, startPos.y), null));

    private Cell GetEndRight(Vector2Int startPos)
        => (_board.GetValueOrDefault(new Vector2Int(startPos.x + 2, startPos.y), null));

    public bool IsValidMovement() 
    {
        // jump cell becomes null when start and end positions don't indicate a valid movement
        SetJump();
        return  (_start.GetValue() == Cell.CellValue.Peg || _start.GetValue() == Cell.CellValue.Selected) && 
                (_jump != null && _jump.GetValue() == Cell.CellValue.Peg) &&
                (_end.GetValue() == Cell.CellValue.Selected || _end.GetValue() == Cell.CellValue.Empty || _end.GetValue() == Cell.CellValue.Predicted);
    }
}
