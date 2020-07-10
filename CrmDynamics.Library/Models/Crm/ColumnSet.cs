using System.Collections.Generic;

namespace CrmDynamics.Library.Models.Crm
{
    public class ColumnSet
    {
        private bool _allColumns;

        private List<string> _columns;

        public ColumnSet(bool allColumns = false)
        {
            _allColumns = allColumns;
        }

        public ColumnSet(params string[] columns)
        {
            _columns = new List<string>(columns);
        }
    }
}
