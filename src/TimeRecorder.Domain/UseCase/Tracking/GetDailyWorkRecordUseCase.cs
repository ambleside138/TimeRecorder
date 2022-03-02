using System;
using System.Collections.Generic;
using System.Text;
using TimeRecorder.Domain.Domain.Tracking;
using TimeRecorder.Domain.Domain.Tracking.Reports;
using TimeRecorder.Domain.UseCase.Tracking.Reports;

namespace TimeRecorder.Domain.UseCase.Tracking;

/// <summary>
/// 指定日の作業時間を取得するUseCaseを表します
/// </summary>
public class GetDailyWorkRecordUseCase
{
    private readonly IDailyWorkRecordQueryService _DailyWorkRecordQueryService;

    public GetDailyWorkRecordUseCase(IDailyWorkRecordQueryService dailyWorkRecordQueryService)
    {
        _DailyWorkRecordQueryService = dailyWorkRecordQueryService;
    }

    public DailyWorkRecordHeader Select(string ymd)
    {
        return new DailyWorkRecordHeader();
    }
}
