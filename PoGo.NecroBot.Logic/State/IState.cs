#region using directives

using System.Threading.Tasks;

#endregion

namespace PoGo.NecroBot.Logic.State
{
    public interface IState
    {
        Task<IState> Execute(Session ctx, StateMachine machine);
    }
}