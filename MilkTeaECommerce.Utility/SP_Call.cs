using Dapper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MilkTeaECommerce.Utility
{
    public static class SP_Call
    {
        public static JArray ExecuteJson(string proceduceName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Server=dbms.czhgxs9hoqoj.ap-southeast-1.rds.amazonaws.com,1433;Database=MilkTeaEcommerce;MultipleActiveResultSets=true;User Id=admin;Password=Hoilamj0123!"))
            {
                sqlConnection.Open();
                try
                {
                    var obj = sqlConnection.ExecuteReader(proceduceName, param,
                        commandType: System.Data.CommandType.StoredProcedure);
                    var r = "";
                    while (obj.Read())
                    {
                        var a = obj.GetValue(0);
                        r += a.ToString();

                    }
                    return JArray.Parse(r);
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }
    }
}
