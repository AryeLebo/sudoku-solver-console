using SudokuSolver;

Console.WriteLine("Please input the board code... ");

var boardCode = Console.ReadLine();

if (boardCode is not null)
{
    var board = new Board(new int[][]
                {
                    new[] { 0, 8, 0, 4, 0, 0, 3, 0, 2 },
                    new[] { 0, 0, 0, 0, 0, 0, 9, 5, 0 },
                    new[] { 2, 0, 9, 0, 0, 0, 6, 0, 4 },
                    new[] { 0, 0, 2, 0, 8, 0, 1, 9, 5 },
                    new[] { 1, 9, 0, 0, 0, 0, 8, 0, 0 },
                    new[] { 5, 0, 8, 0, 0, 3, 0, 4, 0 },
                    new[] { 0, 0, 0, 0, 4, 0, 0, 8, 0 },
                    new[] { 4, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 5, 0, 0, 0, 6, 0, 0, 0 },
                });
    var boardIsSolved = board.Solve();
    if (boardIsSolved)
    {
        Console.WriteLine(string.Join('\n', board.SudokuBoard.Select(row => string.Join(',', row.Select(i => $"{(int)i}")))));
    }
}
