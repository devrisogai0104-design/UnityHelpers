using UnityEngine;

namespace IRCore.UnityHelpers
{
    public class BaseCanvas : MonoBehaviour, ISePlayer
    {
        public virtual void PlaySe(AudioClip audioClip)
        {
            throw new System.NotImplementedException();
        }
    }
}
