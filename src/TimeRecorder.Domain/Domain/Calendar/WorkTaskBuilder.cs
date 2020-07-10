using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Calendar
{
    class WorkTaskBuilder
    {
        private readonly WorkTaskBuilderConfig _Config;

        private readonly ScheduleTitleMap[] _ScheduleTitleMaps;

        public WorkTaskBuilder(WorkTaskBuilderConfig config, ScheduleTitleMap[] maps)
        {
            _Config = config;
            _ScheduleTitleMaps = maps ?? new ScheduleTitleMap[0];
        }

        public WorkTask Build(ScheduledEvent scheduledEvent)
        {
            var oTask = WorkTask.FromScheduledEvent(scheduledEvent);

            var eventConfig = _Config.EventMappers.FirstOrDefault(t => t.EventKind == scheduledEvent.Kind);
            if(eventConfig != null)
            {
                oTask.ProcessId = new Identity<WorkProcesses.WorkProcess>(eventConfig.WorkProcessId);
            }

            if(_ScheduleTitleMaps.Any())
            {
                var mapConfig = _ScheduleTitleMaps.FirstOrDefault(t => t.ScheduleTitle == oTask.Title);
                if(mapConfig != null)
                {
                    oTask.Title = mapConfig.ScheduleTitle;

                    if (string.IsNullOrEmpty(mapConfig.MapTitle) == false)
                        oTask.Title = mapConfig.MapTitle;

                    oTask.TaskCategory = mapConfig.TaskCategory;
                    oTask.ProductId = new Identity<Products.Product>(mapConfig.ProductId);
                    oTask.ProcessId = new Identity<WorkProcesses.WorkProcess>(mapConfig.WorkProcessId);
                    oTask.ClientId = new Identity<Clients.Client>(mapConfig.ClientId);
                }
            }
            else
            {
                var targetConfig = _Config.TitleMappers.FirstOrDefault(t => t.ScheduleTitle == oTask.Title);
                if (targetConfig != null)
                {
                    oTask.TaskCategory = targetConfig.TaskCategory;
                    oTask.ProductId = new Identity<Products.Product>(targetConfig.ProductId);
                    oTask.ProcessId = new Identity<WorkProcesses.WorkProcess>(targetConfig.WorkProcessId);
                }
            }


            return oTask;
        }

        
    }
}
