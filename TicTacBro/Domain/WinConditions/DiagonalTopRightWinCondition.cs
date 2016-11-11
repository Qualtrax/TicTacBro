namespace TicTacBro.Domain.WinConditions
{
    public class DiagonalTopRightWinCondition : WinCondition
    {
        public DiagonalTopRightWinCondition()
        {
            condition = new[] { 2, 4, 6 };
        }
    }
}