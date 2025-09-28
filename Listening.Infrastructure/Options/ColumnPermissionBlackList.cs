using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure.Options
{
    public class ColumnPermissionBlackList
    {
        public string ColumnName { get; set; }
        public string FullTableName { get; set; }

        public string? Description { get; set; }
    }
}
