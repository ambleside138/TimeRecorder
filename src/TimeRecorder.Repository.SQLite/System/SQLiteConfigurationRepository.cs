using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TimeRecorder.Domain.Domain.System;
using TimeRecorder.Repository.SQLite.System.Dao;

namespace TimeRecorder.Repository.SQLite.System
{
    public class SQLiteConfigurationRepository : IConfigurationRepository
    {
        public ConfigurationItem[] SelectAll()
        {
            ConfigurationItem[] results = null;

            RepositoryAction.Query(c =>
            {
                var dao = new ConfigDao(c, null);

                results = dao.SelectAll().Select(i => i.ConvertToDomainObject()).ToArray();
            });

            return results;
        }

        public void UpdateConfiguration(ConfigurationItem item)
        {
            RepositoryAction.Transaction((c, t) =>
            {
                var dao = new ConfigDao(c, t);
                dao.DeleteInsert(ConfigTableRow.FromDomainObject(item));
            });
        }
    }
}
