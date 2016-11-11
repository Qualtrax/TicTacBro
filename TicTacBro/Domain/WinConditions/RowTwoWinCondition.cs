namespace TicTacBro.Domain.WinConditions
{
    public class RowTwoWinCondition : WinCondition
    {
        public RowTwoWinCondition()
        {
            condition = new[] { 3, 4, 5 };
        }
    }
}