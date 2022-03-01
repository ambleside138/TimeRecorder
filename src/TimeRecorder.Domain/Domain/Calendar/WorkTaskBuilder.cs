using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Schema;
using TimeRecorder.Domain.Domain.Tasks;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.Calendar;

class WorkTaskBuilder
{
    private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

    private readonly WorkTaskBuilderConfig _Config;

    private readonly ScheduleTitleMap[] _ScheduleTitleMaps;

    public WorkTaskBuilder(WorkTaskBuilderConfig config, ScheduleTitleMap[] maps)
    {
        _Config = config;
        _ScheduleTitleMaps = maps ?? Array.Empty<ScheduleTitleMap>();
    }

    public (WorkTask task, ImportedTask imported) Build(ScheduledEvent scheduledEvent)
    {
        _Logger.Info($"starting analyze task for {scheduledEvent.StartTime:yyyy/MM/dd HH:mm～} [{scheduledEvent.Title}]");
        var oTask = WorkTask.FromScheduledEvent(scheduledEvent);

        var eventConfig = _Config.EventMappers.FirstOrDefault(t => t.EventKind == scheduledEvent.Kind);
        if (eventConfig != null)
        {
            oTask.ProcessId = new Identity<WorkProcesses.WorkProcess>(eventConfig.WorkProcessId);
            _Logger.Info($"  By EventConfig > set ProcessId = {oTask.ProcessId.Value}");
        }

        if (_ScheduleTitleMaps.Any())
        {
            _Logger.Info("  Use NewSetting");

            var mapConfig = _ScheduleTitleMaps.FirstOrDefault(t => t.ScheduleTitle.Trim() == oTask.Title.Trim());
            if (mapConfig != null)
            {
                var configLog = JsonSerializer.Serialize(mapConfig, JsonSerializerHelper.DefaultOptions);
                _Logger.Info("  By ScheduleTitleMap > founded config..." + Environment.NewLine + configLog);

                oTask.Title = mapConfig.ScheduleTitle;

                if (string.IsNullOrEmpty(mapConfig.MapTitle) == false)
                    oTask.Title = mapConfig.MapTitle;

                oTask.TaskCategory = mapConfig.TaskCategory;
                oTask.ProductId = new Identity<Products.Product>(mapConfig.ProductId);
                oTask.ProcessId = new Identity<WorkProcesses.WorkProcess>(mapConfig.WorkProcessId);
                oTask.ClientId = new Identity<Clients.Client>(mapConfig.ClientId);
            }
            else
            {
                _Logger.Info("  By ScheduleTitleMap > config not found");
            }
        }
        else
        {
            _Logger.Info("  Use OldSetting");

            var targetConfig = _Config.TitleMappers.FirstOrDefault(t => t.ScheduleTitle == oTask.Title);
            if (targetConfig != null)
            {
                var configLog = JsonSerializer.Serialize(targetConfig, JsonSerializerHelper.DefaultOptions);
                _Logger.Info("  By TitleMapper > founded config..." + Environment.NewLine + configLog);

                oTask.TaskCategory = targetConfig.TaskCategory;
                oTask.ProductId = new Identity<Products.Product>(targetConfig.ProductId);
                oTask.ProcessId = new Identity<WorkProcesses.WorkProcess>(targetConfig.WorkProcessId);
            }
            else
            {
                _Logger.Info("  By TitleMapper > config not found");
            }
        }

        var importedTask = new ImportedTask
        {
            Title = oTask.Title,
            ImportKey = scheduledEvent.Id,
            Source = scheduledEvent.Kind,
        };

        return (oTask, importedTask);
    }


}
