using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace schoolManagerUser1.db
{
    public class DBContext
    {
        private readonly string connectionString;
        public SqlConnection Connection { get; private set; }

        public DBContext()
        {
            // ⚠️ Thay đổi chuỗi kết nối cho phù hợp với SQL Server của bạn
            connectionString = "Data Source=LAPTOP-FKTED273;Initial Catalog=school_management;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;";
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }
    }
}
