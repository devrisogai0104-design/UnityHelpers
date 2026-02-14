using UnityEngine;

namespace IRCore.UnityHelpers.DebugManagement
{
    public abstract class DebugModule : ScriptableObject
    {
        public bool IsEnabled => _isEnabled;
        [SerializeField] private bool _isEnabled = true;
    }
}
