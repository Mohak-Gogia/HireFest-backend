using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HireFest.Services
{
    public sealed class AuthService
    {
        private SqlConnection con = new SqlConnection();

        private static AuthService instance = null;

        public static AuthService GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new AuthService();
                return instance;
            }
        }

        private AuthService()
        {
            ConnectionString();
        }

        public SqlConnection ConnectionString()
        {
            con.ConnectionString = "data source=MOHAK-GOGIA-L; database=HireFest; integrated security = SSPI;";
            return con;
        }
    }
}