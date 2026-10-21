using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.DAL
{
    public class HangDAL
    {
        public List<HangDTO> GetAll()
        {
            var list = new List<HangDTO>();
            string query = "SELECT MaHang, TenHang FROM dbo.Hang ORDER BY TenHang";
            var table = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(new HangDTO
                {
                    MaHang = Convert.ToInt32(row["MaHang"]),
                    TenHang = row["TenHang"].ToString() ?? ""
                });
            }
            return list;
        }

        public bool Insert(string tenHang)
        {
            string query = "INSERT INTO dbo.Hang (TenHang) VALUES (@name)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@name", SqlDbType.NVarChar, 50) { Value = tenHang }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Update(HangDTO hang)
        {
            string query = "UPDATE dbo.Hang SET TenHang = @name WHERE MaHang = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = hang.MaHang },
                new SqlParameter("@name", SqlDbType.NVarChar, 50) { Value = hang.TenHang }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int maHang)
        {
            string query = "DELETE FROM dbo.Hang WHERE MaHang = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maHang }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
