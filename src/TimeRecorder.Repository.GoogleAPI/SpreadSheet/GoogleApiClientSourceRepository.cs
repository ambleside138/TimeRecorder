using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Calendar;
using TimeRecorder.Domain.Domain.Clients;
using TimeRecorder.Domain.Domain.Tasks;

namespace TimeRecorder.Repository.GoogleAPI.SpreadSheet;
public class GoogleApiClientSourceRepository : IClientSourceRepository
{
    public async Task<Client[]> SelectByTaskCategoryAsync(string sourceId, TaskCategory taskCategory)
    {
        // アクセストークン取得
        var credential = await CredentialProvider.GetUserCredentialAsync();
        if (credential == null)
            return Array.Empty<Client>();

        // SpeadSheetAPI
        var service = new SheetsService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "TaskRecorder",
        });

        var range = "案件マスタ!";
        if(taskCategory == TaskCategory.Introduce)
        {
            range += "A2:A";
        }
        else
        {
            range += "B2:B";
        }

        var request = service.Spreadsheets.Values.Get(sourceId, range);

        try
        {
            ValueRange valueRange = request.Execute();
            IList<IList<object>> values = valueRange.Values;

            if (values == null || values.Count == 0)
            {
                return Array.Empty<Client>();
            }

            return values.Select(i => Client.ForSource( i[0].ToString())).ToArray();
        }
        catch (Exception ex)
        {

            throw;
        }


    }
}
