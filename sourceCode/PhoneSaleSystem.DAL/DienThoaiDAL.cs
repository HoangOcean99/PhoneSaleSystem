using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.DAL
{
    public class DienThoaiDAL
    {
        public List<DienThoaiDTO> GetAll()
        {
            var list = new List<DienThoaiDTO>();
            string query = @"
                SELECT dt.MaDT, dt.TenDT, dt.MaHang, h.TenHang, dt.RAM, dt.BoNho, 
                       dt.MauSac, dt.GiaNhap, dt.GiaBan, dt.SoLuong, dt.BaoHanh
                FROM dbo.DienThoai dt
                INNER JOIN dbo.Hang h ON dt.MaHang = h.MaHang
                ORDER BY dt.MaDT DESC";
            
            var table = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(MapRowToDTO(row));
            }
            return list;
        }

        public DienThoaiDTO? GetById(int maDT)
        {
            string query = @"
                SELECT dt.MaDT, dt.TenDT, dt.MaHang, h.TenHang, dt.RAM, dt.BoNho, 
                       dt.MauSac, dt.GiaNhap, dt.GiaBan, dt.SoLuong, dt.BaoHanh
                FROM dbo.DienThoai dt
                INNER JOIN dbo.Hang h ON dt.MaHang = h.MaHang
                WHERE dt.MaDT = @id";
            
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maDT }
            };
            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            if (table.Rows.Count > 0)
            {
                return MapRowToDTO(table.Rows[0]);
            }
            return null;
        }

        public List<DienThoaiDTO> Search(string keyword, int? maHang = null)
        {
            var list = new List<DienThoaiDTO>();
            string query = @"
                SELECT dt.MaDT, dt.TenDT, dt.MaHang, h.TenHang, dt.RAM, dt.BoNho, 
                       dt.MauSac, dt.GiaNhap, dt.GiaBan, dt.SoLuong, dt.BaoHanh
                FROM dbo.DienThoai dt
                INNER JOIN dbo.Hang h ON dt.MaHang = h.MaHang
                WHERE (dt.TenDT LIKE @keyword OR h.TenHang LIKE @keyword OR dt.MauSac LIKE @keyword)";
            
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@keyword", SqlDbType.NVarChar) { Value = $"%{keyword}%" }
            };

            if (maHang.HasValue && maHang.Value > 0)
            {
                query += " AND dt.MaHang = @maHang";
                parameters.Add(new SqlParameter("@maHang", SqlDbType.Int) { Value = maHang.Value });
            }

            query += " ORDER BY dt.MaDT DESC";

            var table = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
            foreach (DataRow row in table.Rows)
            {
                list.Add(MapRowToDTO(row));
            }
            return list;
        }

        public bool Insert(DienThoaiDTO dt)
        {
            string query = @"
                INSERT INTO dbo.DienThoai (TenDT, MaHang, RAM, BoNho, MauSac, GiaNhap, GiaBan, SoLuong, BaoHanh)
                VALUES (@tenDT, @maHang, @ram, @boNho, @mauSac, @giaNhap, @giaBan, @soLuong, @baoHanh)";
            
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@tenDT", SqlDbType.NVarChar, 100) { Value = dt.TenDT },
                new SqlParameter("@maHang", SqlDbType.Int) { Value = dt.MaHang },
                new SqlParameter("@ram", SqlDbType.VarChar, 10) { Value = (object?)dt.RAM ?? DBNull.Value },
                new SqlParameter("@boNho", SqlDbType.VarChar, 20) { Value = (object?)dt.BoNho ?? DBNull.Value },
                new SqlParameter("@mauSac", SqlDbType.NVarChar, 30) { Value = (object?)dt.MauSac ?? DBNull.Value },
                new SqlParameter("@giaNhap", SqlDbType.Decimal) { Value = dt.GiaNhap },
                new SqlParameter("@giaBan", SqlDbType.Decimal) { Value = dt.GiaBan },
                new SqlParameter("@soLuong", SqlDbType.Int) { Value = dt.SoLuong },
                new SqlParameter("@baoHanh", SqlDbType.Int) { Value = dt.BaoHanh }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool Update(DienThoaiDTO dt)
        {
            string query = @"
                UPDATE dbo.DienThoai
                SET TenDT = @tenDT, MaHang = @maHang, RAM = @ram, BoNho = @boNho, MauSac = @mauSac,
                    GiaNhap = @giaNhap, GiaBan = @giaBan, SoLuong = @soLuong, BaoHanh = @baoHanh
                WHERE MaDT = @maDT";
            
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@maDT", SqlDbType.Int) { Value = dt.MaDT },
                new SqlParameter("@tenDT", SqlDbType.NVarChar, 100) { Value = dt.TenDT },
                new SqlParameter("@maHang", SqlDbType.Int) { Value = dt.MaHang },
                new SqlParameter("@ram", SqlDbType.VarChar, 10) { Value = (object?)dt.RAM ?? DBNull.Value },
                new SqlParameter("@boNho", SqlDbType.VarChar, 20) { Value = (object?)dt.BoNho ?? DBNull.Value },
                new SqlParameter("@mauSac", SqlDbType.NVarChar, 30) { Value = (object?)dt.MauSac ?? DBNull.Value },
                new SqlParameter("@giaNhap", SqlDbType.Decimal) { Value = dt.GiaNhap },
                new SqlParameter("@giaBan", SqlDbType.Decimal) { Value = dt.GiaBan },
                new SqlParameter("@soLuong", SqlDbType.Int) { Value = dt.SoLuong },
                new SqlParameter("@baoHanh", SqlDbType.Int) { Value = dt.BaoHanh }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool UpdateSoLuong(int maDT, int soLuongThayDoi, SqlTransaction? transaction = null)
        {
            string query = "UPDATE dbo.DienThoai SET SoLuong = SoLuong + @delta WHERE MaDT = @id AND (SoLuong + @delta) >= 0";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maDT },
                new SqlParameter("@delta", SqlDbType.Int) { Value = soLuongThayDoi }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters, transaction) > 0;
        }

        public bool Delete(int maDT)
        {
            string query = "DELETE FROM dbo.DienThoai WHERE MaDT = @id";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int) { Value = maDT }
            };
            return DatabaseHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        private static DienThoaiDTO MapRowToDTO(DataRow row)
        {
            return new DienThoaiDTO
            {
                MaDT = Convert.ToInt32(row["MaDT"]),
                TenDT = row["TenDT"].ToString() ?? "",
                MaHang = Convert.ToInt32(row["MaHang"]),
                TenHang = row["TenHang"].ToString() ?? "",
                RAM = row["RAM"] != DBNull.Value ? row["RAM"].ToString() : null,
                BoNho = row["BoNho"] != DBNull.Value ? row["BoNho"].ToString() : null,
                MauSac = row["MauSac"] != DBNull.Value ? row["MauSac"].ToString() : null,
                GiaNhap = Convert.ToDecimal(row["GiaNhap"]),
                GiaBan = Convert.ToDecimal(row["GiaBan"]),
                SoLuong = Convert.ToInt32(row["SoLuong"]),
                BaoHanh = Convert.ToInt32(row["BaoHanh"])
            };
        }
    }
}
