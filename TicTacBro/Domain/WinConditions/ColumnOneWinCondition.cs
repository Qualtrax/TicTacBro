namespace TicTacBro.Domain.WinConditions
{
    public class ColumnOneWinCondition : WinCondition
    {
        public ColumnOneWinCondition()
        {
            condition = new[] { 0, 3, 6 };
        }
    }
}