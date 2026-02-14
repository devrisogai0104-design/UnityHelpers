using UnityEngine;

namespace IRCore.UnityHelpers.DebugManagement
{
    public abstract class DebugModuleBase : ScriptableObject
    {
        public bool IsEnabled => _isEnabled;
        [SerializeField] private bool _isEnabled = true;
    }
}
