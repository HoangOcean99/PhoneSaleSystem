using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.DAL
{
    public class TaiKhoanDAL
    {
        public TaiKhoanDTO? Login(string tenDangNhap, string matKhau)
        {
            string query = "SELECT MaTK, TenDangNhap, MatKhau, Quyen FROM dbo.TaiKhoan WHERE TenDangNhap = @username AND MatKhau = @password";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@username", SqlDbType.VarChar) { Value = tenDangNhap },
                new SqlParameter("@password", SqlDbType.VarChar) { Value = matKhau }
            };

            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            if (table.Rows.Count > 0)
            {
                var row = table.Rows[0];
                return new TaiKhoanDTO
                {
                    MaTK = Convert.ToInt32(row["MaTK"]),
                    TenDangNhap = row["TenDangNhap"].ToString() ?? "",
                    MatKhau = row["MatKhau"].ToString() ?? "",
                    Quyen = row["Quyen"].ToString() ?? "NhanVien"
                };
            }
            return null;
        }

        public List<TaiKhoanDTO> GetAll()
        {
            var list = new List<TaiKhoanDTO>();
            string query = "SELECT MaTK, TenDangNhap, MatKhau, Quyen FROM dbo.TaiKhoan ORDER BY MaTK";
            var table = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(new TaiKhoanDTO
                {
                    MaTK = Convert.ToInt32(row["MaTK"]),
                    TenDangNhap = row["TenDangNhap"].ToString() ?? "",
                    MatKhau = row["MatKhau"].ToString() ?? "",
                    Quyen = row["Quyen"].ToString() ?? "NhanVien"
                });
            }
            return list;
        }

        public bool Insert(TaiKhoanDTO tk)
        {
            string query = "INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhau, Quyen) VALUES (@username, @password, @quyen)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@username", SqlDbType.VarChar) { Value = tk.TenDangNhap },
                new SqlParameter("@password", SqlDbType.VarChar) { Value = tk.MatKhau },
                new SqlParameter("@quyen", SqlDbType.VarChar) { Value = tk.Quyen }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdatePassword(int maTK, string matKhauMoi)
        {
            string query = "UPDATE dbo.TaiKhoan SET MatKhau = @newPass WHERE MaTK = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maTK },
                new SqlParameter("@newPass", SqlDbType.VarChar) { Value = matKhauMoi }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int maTK)
        {
            string query = "DELETE FROM dbo.TaiKhoan WHERE MaTK = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maTK }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }
    }
}
