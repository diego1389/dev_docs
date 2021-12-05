using System;
namespace DesignPatterns3.StatePattern.HandmadeStateMachine
{
  public enum State
    {
        OffHook,
        Connecting,
        Connedted,
        OnHold
    }

    public enum Trigger
    {
        CallDialed,
        HangUp,
        CallConnected,
        PlacedOnHold,
        TakeOffHold,
        LeftMessage
    }
}
