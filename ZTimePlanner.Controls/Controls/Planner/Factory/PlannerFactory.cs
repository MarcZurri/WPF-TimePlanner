using ZTimePlanner.Controls.Models;

namespace ZTimePlanner.Controls.Controls.Planner.Factory
{
    internal static class PlannerFactory
    {
        internal static PlannerBase CreatePlanner(PlannerTypes plannerType)
        {
            return plannerType switch
            {
                PlannerTypes.Week => new PlannerWeek(),
                PlannerTypes.WorkWeek => new PlannerWorkWeek(),
                PlannerTypes.Month => new PlannerMonth(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
