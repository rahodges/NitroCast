using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using System.Text;

namespace NitroCast.Core.Support
{
    public class SQLChecker
    {
        static string[][] keywords;

        static SQLChecker()
        {
            keywords = new string[4][];

            keywords[0] = Support.ReservedKeywords.Keywords_SQLServer.Split(',');
            keywords[1] = Support.ReservedKeywords.Keywords_ODBC.Split(',');
            keywords[2] = Support.ReservedKeywords.Keywords_SQLFuture.Split(',');
            keywords[3] = Support.ReservedKeywords.Keywords_JetSQL.Split(',');

            Array.Sort(keywords[0]);
            Array.Sort(keywords[1]);
            Array.Sort(keywords[2]);
            Array.Sort(keywords[3]);
        }

        public static bool KeywordCheck(string value)
        {
            bool sqlServer;
            bool odbc;
            bool sqlFuture;
            bool jetSql;

            return KeywordCheck(value, out sqlServer, out odbc,
                out sqlFuture, out jetSql);
        }

        public static bool KeywordCheck(string value, out bool sqlServer, 
            out bool odbc, out bool sqlFuture, out bool jetSql)
        {   
            int sqlServerIndex;
            int odbcIndex;
            int sqlFutureIndex;
            int jetSqlIndex;

            sqlServerIndex = 
                Array.BinarySearch(keywords[0], value, CaseInsensitiveComparer.Default);
            odbcIndex = 
                Array.BinarySearch(keywords[1], value, CaseInsensitiveComparer.Default);
            sqlFutureIndex =
                Array.BinarySearch(keywords[2], value, CaseInsensitiveComparer.Default);
            jetSqlIndex =
                Array.BinarySearch(keywords[3], value, CaseInsensitiveComparer.Default);

            sqlServer = sqlServerIndex > -1;
            odbc = odbcIndex > -1;
            sqlFuture = sqlFutureIndex > -1;
            jetSql = jetSqlIndex > -1;

            return sqlServer | odbc | sqlFuture | jetSql;
        }
    }
}
