namespace TicTacBro.Domain.WinConditions
{
    public class RowThreeWinCondition : WinCondition
    {
        public RowThreeWinCondition()
        {
            condition = new[] { 6, 7, 8 };
        }
    }
}