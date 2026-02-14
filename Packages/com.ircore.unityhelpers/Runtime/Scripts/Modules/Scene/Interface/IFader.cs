using System.Threading.Tasks;

namespace IRCore.UnityHelpers.Scene
{
    public interface IFader
    {
        Task FadeIn(float duration);
        Task FadeOut(float duration);
        void SetProgress(float progress);
    }
}
