using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public static class BoardCodeParser
    {
        public static SudokuValues GetSudokuValues(int value) => Enum.IsDefined((SudokuValues)value) 
            ? (SudokuValues)value 
            : throw new ArgumentOutOfRangeException(paramName: nameof(value), message: $"Invalid sudoku value", actualValue: value);
    }
}
