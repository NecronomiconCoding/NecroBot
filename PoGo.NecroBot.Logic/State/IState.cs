using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.State
{
    public interface IState
    {
        Task<IState> Execute(Context ctx, StateMachine machine);
    }
}