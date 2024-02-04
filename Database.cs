using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace QLKTX
{
    internal class Database
    {
        //Khai báo biến toàn cục, bạn phải thay đổi chuối kết nối phù hợp
        private string strConnect = "Data Source=ACER\\SQLEXPRESS;Initial Catalog=KTX;Integrated Security=True";

        private SqlConnection sqlConnect = null;

        //Phương thức mở kết nối
        public void OpenConnect()
        {
            sqlConnect = new SqlConnection(strConnect);
            if (sqlConnect.State != ConnectionState.Open)
                sqlConnect.Open();
        }

        //Phương thức đóng kết nối
        public void CloseConnect()
        {
            if (sqlConnect.State != ConnectionState.Closed)
            {
                sqlConnect.Close();
                sqlConnect.Dispose(); //huỷ đối tượng
            }
        }

        //Phương thức thực thi câu lệnh Select trả về một DataTable
        public DataTable DataReader(string sqlSelct)
        {
            DataTable tblData = new DataTable();
            OpenConnect();
            SqlDataAdapter sqlData = new SqlDataAdapter(sqlSelct, sqlConnect);
            sqlData.Fill(tblData);// Đổ dữ liệu vào sql
            CloseConnect();
            return tblData;
        }

        //Phương thức thực hiện câu lệnh dạng insert,update,delete
        public void DataChange(string sql)
        {
            OpenConnect();
            SqlCommand commandsql = new SqlCommand();
            commandsql.Connection = sqlConnect;//Chỉ định đối tượng kết nối
            commandsql.CommandText = sql;//Truyền lệnh sql cho thuộc tính CommandText
            commandsql.ExecuteNonQuery();//thực thi câu lệnh bằng phương thức EX
            CloseConnect();
            commandsql.Dispose();//Hủy đối tượng commandsql
        }
    }
}