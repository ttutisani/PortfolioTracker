using Gherkin.Ast;
using System;
using System.Linq;

namespace PortfolioTracker.AcceptanceTests
{
    internal static class DataTableExtensions
    {
        public static TValue GetValue<TValue>(
            this DataTable dataTable, 
            int rowIndex,
            string columnName)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable));

            if (rowIndex < 0)
                throw new ArgumentException($"`{rowIndex}` must be 0 or positive.");

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException(nameof(columnName));

            var rows = dataTable.Rows?.ToList();
            if (rows == null || rows.Count == 0)
                throw new ArgumentException("Rows cannot be empty.");

            var header = rows[0]?.Cells?.Select(c => c.Value)?.ToList();
            if (header == null || header.Count < (rowIndex + 2))
                throw new ArgumentException($"Number of rows is too few to retrieve row at index `{rowIndex}`.");

            var columnIndex = header.IndexOf(columnName);
            if (columnIndex < 0)
                throw new ArgumentException($"Cannot find column `{columnName}`.");

            var row = rows[rowIndex + 1]?.Cells?.Select(c => c.Value)?.ToList();
            if (row == null || row.Count < (columnIndex + 1))
                throw new ArgumentException($"Row at index `{rowIndex}` has too few cells to retrieve value at index `{columnIndex}`.");

            return (TValue)Convert.ChangeType(row[columnIndex], typeof(TValue));
        }
    }
}
