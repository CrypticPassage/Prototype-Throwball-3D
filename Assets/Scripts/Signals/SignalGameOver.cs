namespace Signals
{
    public class SignalGameOver
    {
        public bool IsWin;

        public SignalGameOver(bool isWin)
        {
            IsWin = isWin;
        }
    }
}