using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeRecorder.Domain.Domain.Tasks;

namespace TimeRecorder.Domain.Domain.Clients;
public interface IClientSourceRepository
{
    public Task<Client[]> SelectByTaskCategoryAsync(string sourceId, TaskCategory taskCategory);
}
