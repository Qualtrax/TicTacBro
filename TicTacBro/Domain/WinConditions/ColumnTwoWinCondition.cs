namespace TicTacBro.Domain.WinConditions
{
    public class ColumnTwoWinCondition : WinCondition
    {
        public ColumnTwoWinCondition()
        {
            condition = new[] { 1, 4, 7 };
        }
    }
}