using MojoCase.Crowd;
using UnityEngine;

namespace MojoCase.Game
{
    public class FireRateGate : Gate
    {
        public override void ApplyGateEffect(CrowdManager crowdManager)
        {
            base.ApplyGateEffect(crowdManager);
            
            crowdManager.AddFireRateModifier(GateValue);
        }
    }
}