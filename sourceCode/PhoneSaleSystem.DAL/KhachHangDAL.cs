using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.DAL
{
    public class KhachHangDAL
    {
        public List<KhachHangDTO> GetAll()
        {
            var list = new List<KhachHangDTO>();
            string query = "SELECT MaKH, HoTen, SDT, DiaChi FROM dbo.KhachHang ORDER BY MaKH DESC";
            var table = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(MapRowToDTO(row));
            }
            return list;
        }

        public KhachHangDTO? GetById(int maKH)
        {
            string query = "SELECT MaKH, HoTen, SDT, DiaChi FROM dbo.KhachHang WHERE MaKH = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maKH }
            };
            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            if (table.Rows.Count > 0)
            {
                return MapRowToDTO(table.Rows[0]);
            }
            return null;
        }

        public List<KhachHangDTO> Search(string keyword)
        {
            var list = new List<KhachHangDTO>();
            string query = "SELECT MaKH, HoTen, SDT, DiaChi FROM dbo.KhachHang WHERE HoTen LIKE @keyword OR SDT LIKE @keyword OR DiaChi LIKE @keyword ORDER BY MaKH DESC";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@keyword", SqlDbType.NVarChar) { Value = $"%{keyword}%" }
            };
            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            foreach (DataRow row in table.Rows)
            {
                list.Add(MapRowToDTO(row));
            }
            return list;
        }

        public bool Insert(KhachHangDTO kh, out int generatedId)
        {
            generatedId = 0;
            string query = "INSERT INTO dbo.KhachHang (HoTen, SDT, DiaChi) OUTPUT INSERTED.MaKH VALUES (@hoTen, @sdt, @diaChi)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@hoTen", SqlDbType.NVarChar, 100) { Value = kh.HoTen },
                new SqlParameter("@sdt", SqlDbType.VarChar, 15) { Value = kh.SDT },
                new SqlParameter("@diaChi", SqlDbType.NVarChar, 200) { Value = (object?)kh.DiaChi ?? DBNull.Value }
            };
            var result = DatabaseHelper.ExecuteScalar(query, parameters);
            if (result != null && int.TryParse(result.ToString(), out int id))
            {
                generatedId = id;
                return true;
            }
            return false;
        }

        public bool Update(KhachHangDTO kh)
        {
            string query = "UPDATE dbo.KhachHang SET HoTen = @hoTen, SDT = @sdt, DiaChi = @diaChi WHERE MaKH = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = kh.MaKH },
                new SqlParameter("@hoTen", SqlDbType.NVarChar, 100) { Value = kh.HoTen },
                new SqlParameter("@sdt", SqlDbType.VarChar, 15) { Value = kh.SDT },
                new SqlParameter("@diaChi", SqlDbType.NVarChar, 200) { Value = (object?)kh.DiaChi ?? DBNull.Value }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int maKH)
        {
            string query = "DELETE FROM dbo.KhachHang WHERE MaKH = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maKH }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private static KhachHangDTO MapRowToDTO(DataRow row)
        {
            return new KhachHangDTO
            {
                MaKH = Convert.ToInt32(row["MaKH"]),
                HoTen = row["HoTen"].ToString() ?? "",
                SDT = row["SDT"].ToString() ?? "",
                DiaChi = row["DiaChi"] != DBNull.Value ? row["DiaChi"].ToString() : null
            };
        }
    }
}
