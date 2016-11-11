namespace TicTacBro.Domain.WinConditions
{
    public class DiagonalTopLeftWinCondition : WinCondition
    {
        public DiagonalTopLeftWinCondition()
        {
            condition = new[] { 0, 4, 8 };
        }
    }
}