using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Board
    {
        private SudokuValues[][] _sudokuBoard;
        public IList<IList<SudokuValues>> SudokuBoard 
        { 
            get => Array.AsReadOnly<IList<SudokuValues>>(_sudokuBoard.Select(i => Array.AsReadOnly(i)).ToArray()); 
        }
        
        public int ValueLength { get; }

        public Board(int[][] sudokuBoard) 
        {
            _sudokuBoard = sudokuBoard.Select(i => i.Select(x => BoardCodeParser.GetSudokuValues(x)).ToArray()).ToArray();
            ValueLength = Enum.GetValues<SudokuValues>().Where(i => i != SudokuValues.Empty).Count();
        }

        private bool? isValidBoard;
        public bool IsValidBoard
        {
            get
            {
                if (!isValidBoard.HasValue)
                    isValidBoard = ValidateBoard(_sudokuBoard);
                return isValidBoard.Value;
            }
        }

        public SudokuValues[] GetColumn(int col) => _sudokuBoard.Select(row => row[col]).ToArray();
        private SudokuValues[] GetArea((int start, int end) rowDimensions, (int start, int end) colDimensions)
        {
            SudokuValues[] square = _sudokuBoard
                .Where((row, i) => i >= rowDimensions.start && i < rowDimensions.end)
                .SelectMany(row => row.Where((col, x) => x >= colDimensions.start && x < colDimensions.end)).ToArray();
            return square;
        }
        public SudokuValues[] GetSquare((int row, int col) valuePosition)
        {
            var rowSquare = (valuePosition.row / 3) switch
            {
                0 => (0, 3),
                1 => (3, 6),
                2 => (6, 9),
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(valuePosition.row), message: "Unknown row position!", actualValue: valuePosition.row)
            };
            var colSquare = (valuePosition.col / 3) switch
            {
                0 => (0, 3),
                1 => (3, 6),
                2 => (6, 9),
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(valuePosition.col), message: "Unknown column position!", actualValue: valuePosition.col)
            };

            return GetArea(rowSquare, colSquare);
        }

        public bool NotInRow(int row, SudokuValues sudokuValue) 
        {
            int count = _sudokuBoard[row].Count(i => i == sudokuValue);
            return count switch
            {
                0 => true,
                1 => false,
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(sudokuValue), message: "Value appears more than once!", actualValue: sudokuValue)
            };
        }

        public bool NotInColumn(int col, SudokuValues sudokuValue)
        {
            int count = GetColumn(col).Count(i => i == sudokuValue);
            return count switch
            {
                0 => true,
                1 => false,
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(sudokuValue), message: "Value appears more than once!", actualValue: sudokuValue)
            };
        }

        public bool NotInSquare((int row, int col) valuePosition, SudokuValues sudokuValue)
        {
            int count = GetSquare(valuePosition).Count(i => i == sudokuValue);
            return count switch
            {
                0 => true,
                1 => false,
                _ => throw new ArgumentOutOfRangeException(paramName: nameof(sudokuValue), message: "Value appears more than once!", actualValue: sudokuValue)
            };
        }

        public bool ValidPosition((int row, int col) valuePosition, SudokuValues sudokuValue) => NotInRow(valuePosition.row, sudokuValue) && NotInColumn(valuePosition.col, sudokuValue) && NotInSquare(valuePosition, sudokuValue);



        public bool ValidateBoard(SudokuValues[][] board)
        {
            if (board.SelectMany(i => i).Count() != (ValueLength * ValueLength))
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(board), message: "Invalid board!");
            }
            for (int i = 0; i < board.Length; i++)
            {
                SudokuValues[] row = board[i];
                for (int x = 0; x < row.Length; x++)
                {
                    SudokuValues value = BoardCodeParser.GetSudokuValues((int)board[i][x]);
                    if (value == SudokuValues.Empty) continue;

                    _ = NotInRow(i, value) & NotInColumn(x, value) & NotInSquare((x, i), value);
                }
            }

            return true;
        }

        public bool IsBoardSolved()
        {
            return _sudokuBoard.Any(i => i.Any(x => x == SudokuValues.Empty)) == false;
        }

        public bool Solve()
        {
            if (IsValidBoard)
            {
                TrySolve(_sudokuBoard, 1);
                return IsBoardSolved();
            }
            return false;
        }

        private bool TrySolve(SudokuValues[][] boardToFill, int currentNumber) 
        {
            int row = Array.FindIndex(boardToFill, i => i.Contains(SudokuValues.Empty));
            int col = row == -1 ? -1 : Array.FindIndex(boardToFill[row], i => i == SudokuValues.Empty);

            if (row != -1 && col != -1)
            {
                var currentSudokuValue = BoardCodeParser.GetSudokuValues(currentNumber);
                if (ValidPosition((row, col), currentSudokuValue))
                {
                    boardToFill[row][col] = currentSudokuValue;
                    if (TrySolve(boardToFill, 1) == false)
                    {
                        boardToFill[row][col] = SudokuValues.Empty;
                    }
                }

                if (currentNumber + 1 > ValueLength)
                {
                    return false;
                }

                TrySolve(boardToFill, ++currentNumber);

                if (IsBoardSolved() == false)
                {
                    return false;
                }
            }

            return true;
        }

    }

    public enum SudokuValues
    {
        Empty = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
    }
}
