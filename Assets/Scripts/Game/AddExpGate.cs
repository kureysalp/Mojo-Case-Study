using System;
using MojoCase.Crowd;

namespace MojoCase.Game
{
    public class AddExpGate : Gate
    {
        public static event Action<int> OnAddExp;
        
        public override void ApplyGateEffect(CrowdManager crowdManager)
        {
            base.ApplyGateEffect(crowdManager);
            OnAddExp?.Invoke(GateValue);
        }
    }
}