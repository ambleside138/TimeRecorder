using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Utility;

namespace TimeRecorder.Domain.Domain.WorkProcesses;

/// <summary>
/// 工程 を表します
/// </summary>
public class WorkProcess : Entity<WorkProcess>
{
    public Identity<WorkProcess> Id { get; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// 無効フラグ
    /// </summary>
    public bool Invalid { get; }


    /// <summary>
    /// Domain層内のみで、titleのみでの生成を許可する
    /// </summary>
    /// <param name="title"></param>
    internal WorkProcess(string title)
        : this(Identity<WorkProcess>.Temporary, title, false) { }


    public WorkProcess(Identity<WorkProcess> identity, string title, bool invalid = false)
    {
        Id = identity;
        Title = title;
        Invalid = invalid;
    }

    protected override IEnumerable<object> GetIdentityValues()
    {
        yield return Id;
    }
}
