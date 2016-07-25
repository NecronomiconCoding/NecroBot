namespace PoGo.NecroBot.Logic.State
{
    public interface IState
    {
        IState Execute(Context ctx, StateMachine machine);
    }
}
