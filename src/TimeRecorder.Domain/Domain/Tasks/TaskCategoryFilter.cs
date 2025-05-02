using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeRecorder.Domain.Domain.Tasks;

// 本来はFlagsで管理したかったが、DBにValueをいれてしまってるせいで
// 値の変更がしずらかったので別途フィルタ用のクラスを用意することにした 

/// <summary>
/// フィルタ
/// </summary>
public class TaskCategoryFilter
{
    private List<TaskCategory> _Targets = new();

    public static TaskCategoryFilter Empty => new TaskCategoryFilter();

    public static TaskCategoryFilter CreateFromString(string filterText)
    {
        var result = new TaskCategoryFilter();

        if (string.IsNullOrEmpty(filterText))
        {
            return result;
        }

        var filters = filterText.Split(",");

        foreach (var item in Enum.GetValues(typeof(TaskCategory))
                                 .OfType<TaskCategory>()
                                 .Where(c => c != TaskCategory.UnKnown))
        { 
            if (filters.Contains(Enum.GetName(item)))
            {
                result._Targets.Add(item);
            }
        }

        return result;
    }

    public bool Contains(TaskCategory taskCategory)
    {
        return _Targets.Any() == false
                || _Targets.Contains(taskCategory);
    }

    public override string ToString()
    {
        return string.Join(",", _Targets);
    }
}
