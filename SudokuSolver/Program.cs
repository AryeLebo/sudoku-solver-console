using SudokuSolver;

Console.WriteLine("Please input the board code... ");

var boardCode = Console.ReadLine();

if (boardCode is not null)
{
    var board = new Board(new int[][]
                {
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                });
    var boardIsSolved = board.Solve();
    if (boardIsSolved)
    {
        Console.WriteLine(String.Join('\n', board.SudokuBoard.Select(row => string.Join(',', row.Select(i => $"{(int)i}")))));
    }
}
