using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace FtB_Common.Storage
{
    public static class TableStorageUtils
    {
        public static string GetStartsWithFilter(this string columnName, string startsWith)
        {
            var length = startsWith.Length - 1;
            var nextChar = startsWith[length] + 1;

            var startWithEnd = startsWith.Substring(0, length) + (char)nextChar;
            var filter = TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition(columnName, QueryComparisons.GreaterThanOrEqual, startsWith),
                TableOperators.And,
                TableQuery.GenerateFilterCondition(columnName, QueryComparisons.LessThan, startWithEnd));

            return filter;
        }
    }
}
