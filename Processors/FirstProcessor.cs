using OpenTelemetry;
using System.Diagnostics;

namespace Lb13.Processors
{
    public class FirstProcessor : BaseProcessor<Activity>
    {
        public override void OnStart(Activity activity)
        {
            Console.WriteLine($"Start Id: {activity.Id}");
            Console.WriteLine($"Start: {activity.DisplayName} with status - {activity.Status}");

            foreach (var tag in activity.Tags)
            {
                Console.WriteLine($"Attribute: {tag.Key} = {tag.Value}");
            }

            Console.WriteLine($"Activity Context:  {activity.Context}");
        }

        public override void OnEnd(Activity activity)
        {
            Console.WriteLine($"End Id: {activity.Id}");
            Console.WriteLine($"End: {activity.DisplayName} with status - {activity.Status}");
        }
    }
}
