using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listening.Infrastructure.Options
{
    public class RowPermissionList
    {
     
        public string FullTableName { get;  set; }
        public string? Description { get; set; }
        public RowDataScopeEnum DataScopeType { get;  set; }
        public string? ScopeField { get;  set; }
        public string? ScopeValue { get;  set; }
        public RowDataAllowOperateEnum RowDataAllowOperateType { get; set; } = RowDataAllowOperateEnum.All;

    }

    public enum RowDataScopeEnum
    {
        None = 0,
        Personal = 1,
        Department = 2,
        DepartmentAndSubordinates = 3,
        Custom = 4,
    }

    [Flags]
    public enum RowDataAllowOperateEnum
    {
        Read = 1,
        Edit = 2,
        Delete = 4,
        All = Read | Edit | Delete
    }
}
