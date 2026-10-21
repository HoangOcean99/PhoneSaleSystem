using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.DAL
{
    public class ThongKeDAL
    {
        public List<DoanhThuDTO> GetDoanhThu(DateTime tuNgay, DateTime denNgay)
        {
            var list = new List<DoanhThuDTO>();
            string query = @"
                SELECT CAST(NgayLap AS DATE) AS Ngay, COUNT(MaHD) AS SoHoaDon, SUM(TongTien) AS DoanhThu
                FROM dbo.HoaDon
                WHERE NgayLap >= @tuNgay AND NgayLap <= @denNgay
                GROUP BY CAST(NgayLap AS DATE)
                ORDER BY Ngay ASC";

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@tuNgay", SqlDbType.DateTime) { Value = tuNgay },
                new SqlParameter("@denNgay", SqlDbType.DateTime) { Value = denNgay }
            };

            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            foreach (DataRow row in table.Rows)
            {
                list.Add(new DoanhThuDTO
                {
                    Ngay = Convert.ToDateTime(row["Ngay"]),
                    SoHoaDon = Convert.ToInt32(row["SoHoaDon"]),
                    DoanhThu = Convert.ToDecimal(row["DoanhThu"])
                });
            }
            return list;
        }

        public List<SanPhamBanChayDTO> GetTopSanPhamBanChay(int top = 5)
        {
            var list = new List<SanPhamBanChayDTO>();
            string query = $@"
                SELECT TOP ({top})
                    dt.MaDT, dt.TenDT, h.TenHang,
                    SUM(ct.SoLuong) AS TongDaBan,
                    SUM(ct.ThanhTien) AS TongDoanhThu
                FROM dbo.ChiTietHoaDon ct
                INNER JOIN dbo.DienThoai dt ON ct.MaDT = dt.MaDT
                INNER JOIN dbo.Hang h ON dt.MaHang = h.MaHang
                GROUP BY dt.MaDT, dt.TenDT, h.TenHang
                ORDER BY TongDaBan DESC";

            var table = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(new SanPhamBanChayDTO
                {
                    MaDT = Convert.ToInt32(row["MaDT"]),
                    TenDT = row["TenDT"].ToString() ?? "",
                    TenHang = row["TenHang"].ToString() ?? "",
                    TongDaBan = Convert.ToInt32(row["TongDaBan"]),
                    TongDoanhThu = Convert.ToDecimal(row["TongDoanhThu"])
                });
            }
            return list;
        }

        public List<TonKhoDTO> GetBaoCaoTonKho()
        {
            var list = new List<TonKhoDTO>();
            string query = @"
                SELECT dt.MaDT, dt.TenDT, h.TenHang, dt.RAM, dt.BoNho, dt.MauSac, dt.GiaBan, dt.SoLuong AS TonKho,
                       CASE 
                           WHEN dt.SoLuong = 0 THEN N'Hết hàng'
                           WHEN dt.SoLuong <= 5 THEN N'Sắp hết hàng'
                           ELSE N'Còn hàng'
                       END AS TrangThai
                FROM dbo.DienThoai dt
                INNER JOIN dbo.Hang h ON dt.MaHang = h.MaHang
                ORDER BY dt.SoLuong ASC";

            var table = DatabaseHelper.ExecuteQuery(query);
            foreach (DataRow row in table.Rows)
            {
                list.Add(new TonKhoDTO
                {
                    MaDT = Convert.ToInt32(row["MaDT"]),
                    TenDT = row["TenDT"].ToString() ?? "",
                    TenHang = row["TenHang"].ToString() ?? "",
                    RAM = row["RAM"] != DBNull.Value ? row["RAM"].ToString() : null,
                    BoNho = row["BoNho"] != DBNull.Value ? row["BoNho"].ToString() : null,
                    MauSac = row["MauSac"] != DBNull.Value ? row["MauSac"].ToString() : null,
                    GiaBan = Convert.ToDecimal(row["GiaBan"]),
                    TonKho = Convert.ToInt32(row["TonKho"]),
                    TrangThai = row["TrangThai"].ToString() ?? ""
                });
            }
            return list;
        }
    }
}
