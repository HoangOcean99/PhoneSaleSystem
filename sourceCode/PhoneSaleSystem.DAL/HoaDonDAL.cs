using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.DAL
{
    public class HoaDonDAL
    {
        public List<HoaDonDTO> GetAll()
        {
            var list = new List<HoaDonDTO>();
            string query = @"
                SELECT hd.MaHD, hd.MaNV, nv.HoTen AS TenNV, hd.MaKH, kh.HoTen AS TenKH, 
                       kh.SDT AS SDTKH, hd.NgayLap, hd.TongTien
                FROM dbo.HoaDon hd
                INNER JOIN dbo.NhanVien nv ON hd.MaNV = nv.MaNV
                INNER JOIN dbo.KhachHang kh ON hd.MaKH = kh.MaKH
                ORDER BY hd.MaHD DESC";
            
            var table = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(MapRowToDTO(row));
            }
            return list;
        }

        public HoaDonDTO? GetById(int maHD)
        {
            string query = @"
                SELECT hd.MaHD, hd.MaNV, nv.HoTen AS TenNV, hd.MaKH, kh.HoTen AS TenKH, 
                       kh.SDT AS SDTKH, hd.NgayLap, hd.TongTien
                FROM dbo.HoaDon hd
                INNER JOIN dbo.NhanVien nv ON hd.MaNV = nv.MaNV
                INNER JOIN dbo.KhachHang kh ON hd.MaKH = kh.MaKH
                WHERE hd.MaHD = @id";
            
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maHD }
            };
            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            if (table.Rows.Count > 0)
            {
                return MapRowToDTO(table.Rows[0]);
            }
            return null;
        }

        public List<HoaDonDTO> Search(string? keyword = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            var list = new List<HoaDonDTO>();
            string query = @"
                SELECT hd.MaHD, hd.MaNV, nv.HoTen AS TenNV, hd.MaKH, kh.HoTen AS TenKH, 
                       kh.SDT AS SDTKH, hd.NgayLap, hd.TongTien
                FROM dbo.HoaDon hd
                INNER JOIN dbo.NhanVien nv ON hd.MaNV = nv.MaNV
                INNER JOIN dbo.KhachHang kh ON hd.MaKH = kh.MaKH
                WHERE 1 = 1";
            
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query += " AND (CAST(hd.MaHD AS VARCHAR) LIKE @kw OR nv.HoTen LIKE @kw OR kh.HoTen LIKE @kw OR kh.SDT LIKE @kw)";
                parameters.Add(new SqlParameter("@kw", SqlDbType.NVarChar) { Value = $"%{keyword}%" });
            }

            if (tuNgay.HasValue)
            {
                query += " AND hd.NgayLap >= @tuNgay";
                parameters.Add(new SqlParameter("@tuNgay", SqlDbType.DateTime) { Value = tuNgay.Value });
            }

            if (denNgay.HasValue)
            {
                query += " AND hd.NgayLap <= @denNgay";
                parameters.Add(new SqlParameter("@denNgay", SqlDbType.DateTime) { Value = denNgay.Value });
            }

            query += " ORDER BY hd.MaHD DESC";

            var table = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
            foreach (DataRow row in table.Rows)
            {
                list.Add(MapRowToDTO(row));
            }
            return list;
        }

        public int Insert(HoaDonDTO hd, SqlTransaction? transaction = null)
        {
            string query = @"
                INSERT INTO dbo.HoaDon (MaNV, MaKH, NgayLap, TongTien)
                OUTPUT INSERTED.MaHD
                VALUES (@maNV, @maKH, @ngayLap, @tongTien)";
            
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@maNV", SqlDbType.Int) { Value = hd.MaNV },
                new SqlParameter("@maKH", SqlDbType.Int) { Value = hd.MaKH },
                new SqlParameter("@ngayLap", SqlDbType.DateTime) { Value = hd.NgayLap },
                new SqlParameter("@tongTien", SqlDbType.Decimal) { Value = hd.TongTien }
            };

            var result = DatabaseHelper.ExecuteScalar(query, parameters, transaction);
            if (result != null && int.TryParse(result.ToString(), out int maHD))
            {
                return maHD;
            }
            return 0;
        }

        public bool Delete(int maHD)
        {
            string query = "DELETE FROM dbo.HoaDon WHERE MaHD = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maHD }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private static HoaDonDTO MapRowToDTO(DataRow row)
        {
            return new HoaDonDTO
            {
                MaHD = Convert.ToInt32(row["MaHD"]),
                MaNV = Convert.ToInt32(row["MaNV"]),
                TenNV = row["TenNV"] != DBNull.Value ? row["TenNV"].ToString() : "",
                MaKH = Convert.ToInt32(row["MaKH"]),
                TenKH = row["TenKH"] != DBNull.Value ? row["TenKH"].ToString() : "",
                SDTKH = row["SDTKH"] != DBNull.Value ? row["SDTKH"].ToString() : "",
                NgayLap = Convert.ToDateTime(row["NgayLap"]),
                TongTien = Convert.ToDecimal(row["TongTien"])
            };
        }
    }
}
