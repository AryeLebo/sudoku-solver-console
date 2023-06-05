using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Board
    {
        public SudokuValues[][] SudokuBoard 
        { 
            get; 
        }

        public Board(int[][] sudokuBoard) 
        {
            SudokuBoard = sudokuBoard.Select(i => i.Select(x => BoardCodeParser.GetSudokuValues(x)).ToArray()).ToArray();
        }

        private bool? isValidBoard;
        public bool IsValidBoard
        {
            get
            {
                if (!isValidBoard.HasValue)
                    isValidBoard = ValidateBoard(SudokuBoard);
                return isValidBoard.Value;
            }
        }

        public SudokuValues[] GetColumn(int col) => SudokuBoard.Select(row => row[col]).ToArray();
        private SudokuValues[] GetArea((int start, int end) rowDimensions, (int start, int end) colDimensions)
        {
            SudokuValues[] square = SudokuBoard
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
                _ => throw new ArgumentException(paramName: nameof(valuePosition), message: "Position needs to be in 3x3 square")
            };
            var colSquare = (valuePosition.col / 3) switch
            {
                0 => (0, 3),
                1 => (3, 6),
                2 => (6, 9),
                _ => throw new ArgumentException(paramName: nameof(valuePosition), message: "Position needs to be in 3x3 square")
            };

            return GetArea(rowSquare, colSquare);
        }

        public bool NotInRow(int row, SudokuValues sudokuValue) 
        {
            int count = SudokuBoard[row].Count(i => i == sudokuValue);
            return count switch
            {
                0 => true,
                1 => false,
                _ => throw new ArgumentException(paramName: nameof(sudokuValue), message: "Invalid Board!")
            };
        }

        public bool NotInColumn(int col, SudokuValues sudokuValue)
        {
            int count = GetColumn(col).Count(i => i == sudokuValue);
            return count switch
            {
                0 => true,
                1 => false,
                _ => throw new ArgumentException(paramName: nameof(sudokuValue), message: "Invalid Board!")
            };
        }

        public bool NotInSquare((int row, int col) valuePosition, SudokuValues sudokuValue)
        {
            int count = GetSquare(valuePosition).Count(i => i == sudokuValue);
            return count switch
            {
                0 => true,
                1 => false,
                _ => throw new ArgumentException(paramName: nameof(sudokuValue), message: "Invalid Board!")
            };
        }

        public bool ValidPosition((int row, int col) valuePosition, SudokuValues sudokuValue) => NotInRow(valuePosition.row, sudokuValue) && NotInColumn(valuePosition.col, sudokuValue) && NotInSquare(valuePosition, sudokuValue);



        public bool ValidateBoard(SudokuValues[][] board)
        {
            for (int i = 0; i < board.Length; i++)
            {
                SudokuValues[] row = board[i];
                for (int x = 0; x < row.Length; x++)
                {
                    SudokuValues value = board[i][x];
                    if (value == SudokuValues.Empty) continue;

                    if (Enum.IsDefined(value) == false)
                        return false;
                    ValidPosition((i, x), value);
                }
            }

            return true;
        }

        public bool IsBoardSolved()
        {
            return SudokuBoard.Any(i => i.Any(x => x == SudokuValues.Empty)) == false;
        }

        public bool Solve()
        {
            if (IsValidBoard)
            {
                TrySolve(SudokuBoard, 1);
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

                if (currentNumber + 1 > boardToFill.Length)
                {
                    return false;
                }

                TrySolve(boardToFill, ++currentNumber);

                if (boardToFill.Any(i => i.Any(x => x == SudokuValues.Empty)))
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
