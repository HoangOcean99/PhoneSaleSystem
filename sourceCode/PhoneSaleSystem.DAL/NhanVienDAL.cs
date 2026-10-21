using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.DAL
{
    public class NhanVienDAL
    {
        public List<NhanVienDTO> GetAll()
        {
            var list = new List<NhanVienDTO>();
            string query = "SELECT MaNV, HoTen, SDT, DiaChi, ChucVu, MaTK FROM dbo.NhanVien ORDER BY MaNV DESC";
            var table = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(MapRowToDTO(row));
            }
            return list;
        }

        public NhanVienDTO? GetById(int maNV)
        {
            string query = "SELECT MaNV, HoTen, SDT, DiaChi, ChucVu, MaTK FROM dbo.NhanVien WHERE MaNV = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maNV }
            };
            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            if (table.Rows.Count > 0)
            {
                return MapRowToDTO(table.Rows[0]);
            }
            return null;
        }

        public NhanVienDTO? GetByMaTK(int maTK)
        {
            string query = "SELECT MaNV, HoTen, SDT, DiaChi, ChucVu, MaTK FROM dbo.NhanVien WHERE MaTK = @maTK";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@maTK", SqlDbType.Int) { Value = maTK }
            };
            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            if (table.Rows.Count > 0)
            {
                return MapRowToDTO(table.Rows[0]);
            }
            return null;
        }

        public List<NhanVienDTO> Search(string keyword)
        {
            var list = new List<NhanVienDTO>();
            string query = "SELECT MaNV, HoTen, SDT, DiaChi, ChucVu, MaTK FROM dbo.NhanVien WHERE HoTen LIKE @keyword OR SDT LIKE @keyword OR ChucVu LIKE @keyword ORDER BY MaNV DESC";
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

        public bool Insert(NhanVienDTO nv)
        {
            string query = "INSERT INTO dbo.NhanVien (HoTen, SDT, DiaChi, ChucVu, MaTK) VALUES (@hoTen, @sdt, @diaChi, @chucVu, @maTK)";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@hoTen", SqlDbType.NVarChar, 100) { Value = nv.HoTen },
                new SqlParameter("@sdt", SqlDbType.VarChar, 15) { Value = (object?)nv.SDT ?? DBNull.Value },
                new SqlParameter("@diaChi", SqlDbType.NVarChar, 200) { Value = (object?)nv.DiaChi ?? DBNull.Value },
                new SqlParameter("@chucVu", SqlDbType.NVarChar, 50) { Value = (object?)nv.ChucVu ?? DBNull.Value },
                new SqlParameter("@maTK", SqlDbType.Int) { Value = (object?)nv.MaTK ?? DBNull.Value }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Update(NhanVienDTO nv)
        {
            string query = "UPDATE dbo.NhanVien SET HoTen = @hoTen, SDT = @sdt, DiaChi = @diaChi, ChucVu = @chucVu, MaTK = @maTK WHERE MaNV = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = nv.MaNV },
                new SqlParameter("@hoTen", SqlDbType.NVarChar, 100) { Value = nv.HoTen },
                new SqlParameter("@sdt", SqlDbType.VarChar, 15) { Value = (object?)nv.SDT ?? DBNull.Value },
                new SqlParameter("@diaChi", SqlDbType.NVarChar, 200) { Value = (object?)nv.DiaChi ?? DBNull.Value },
                new SqlParameter("@chucVu", SqlDbType.NVarChar, 50) { Value = (object?)nv.ChucVu ?? DBNull.Value },
                new SqlParameter("@maTK", SqlDbType.Int) { Value = (object?)nv.MaTK ?? DBNull.Value }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Delete(int maNV)
        {
            string query = "DELETE FROM dbo.NhanVien WHERE MaNV = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maNV }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private static NhanVienDTO MapRowToDTO(DataRow row)
        {
            return new NhanVienDTO
            {
                MaNV = Convert.ToInt32(row["MaNV"]),
                HoTen = row["HoTen"].ToString() ?? "",
                SDT = row["SDT"] != DBNull.Value ? row["SDT"].ToString() : null,
                DiaChi = row["DiaChi"] != DBNull.Value ? row["DiaChi"].ToString() : null,
                ChucVu = row["ChucVu"] != DBNull.Value ? row["ChucVu"].ToString() : null,
                MaTK = row["MaTK"] != DBNull.Value ? Convert.ToInt32(row["MaTK"]) : null
            };
        }
    }
}
