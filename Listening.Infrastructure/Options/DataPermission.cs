using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure.Options
{
    public class DataPermission
    {

        public List<RowPermissionList> RowPermissions { get; set; }
        public List<ColumnPermissionBlackList> ColumnPermissions { get; set; }
    }
}
