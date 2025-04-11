using MojoCase.Crowd;

namespace MojoCase.Game
{
    public class AddWarriorGate : Gate
    {
        public override void ApplyGateEffect(CrowdManager crowdManager)
        {
            base.ApplyGateEffect(crowdManager);
            
            switch (GateValue)
            {
                case > 0:
                    crowdManager.AddWarriorInBulk(GateValue);
                    break;
                case < 0:
                    crowdManager.RemoveWarriorInBulk(GateValue);
                    break;
            }
        }
    }
}