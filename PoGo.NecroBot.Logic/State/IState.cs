using System.Threading.Tasks;

namespace PoGo.NecroBot.Logic.State
{
    public interface IState
    {
        Task<IState> Run();
        Task<IState> OnEnter();
        Task OnExit();
        Task<IState> OnError(System.Exception ex);
    }
}