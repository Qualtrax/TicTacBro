namespace TicTacBro.Domain.WinConditions
{
    public class ColumnThreeWinCondition : WinCondition
    {
        public ColumnThreeWinCondition()
        {
            condition = new[] { 2, 5, 8 };
        }
    }
}