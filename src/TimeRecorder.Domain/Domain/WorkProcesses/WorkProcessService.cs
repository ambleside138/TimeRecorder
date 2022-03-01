using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeRecorder.Domain.Domain.WorkProcesses;

public class WorkProcessService
{
    private readonly IWorkProcessRepository _ProcessRepository;

    public WorkProcessService(IWorkProcessRepository processRepository)
    {
        _ProcessRepository = processRepository;
    }

    // アプリケーション規模によってはSpecificationパターンを利用したほうがいいかもしれない

    /// <summary>
    /// 指定したProcessオブジェクトが登録済みのマスタと重複しているかどうかを判断します
    /// </summary>
    /// <param name="process">テスト対象のProcess</param>
    /// <returns></returns>
    public bool IsDuplicated(WorkProcess process)
    {
        var list = _ProcessRepository.SelectAll();

        return list.Any(i => i.Title == process.Title);
    }
}
