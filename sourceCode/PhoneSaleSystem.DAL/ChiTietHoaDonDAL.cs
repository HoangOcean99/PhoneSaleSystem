using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.DAL
{
    public class ChiTietHoaDonDAL
    {
        public List<ChiTietHoaDonDTO> GetByHoaDonId(int maHD)
        {
            var list = new List<ChiTietHoaDonDTO>();
            string query = @"
                SELECT ct.MaHD, ct.MaDT, dt.TenDT, ct.SoLuong, ct.DonGia, ct.ThanhTien
                FROM dbo.ChiTietHoaDon ct
                INNER JOIN dbo.DienThoai dt ON ct.MaDT = dt.MaDT
                WHERE ct.MaHD = @maHD";
            
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@maHD", SqlDbType.Int) { Value = maHD }
            };

            var table = DatabaseHelper.ExecuteQuery(query, parameters);
            foreach (DataRow row in table.Rows)
            {
                list.Add(new ChiTietHoaDonDTO
                {
                    MaHD = Convert.ToInt32(row["MaHD"]),
                    MaDT = Convert.ToInt32(row["MaDT"]),
                    TenDT = row["TenDT"].ToString() ?? "",
                    SoLuong = Convert.ToInt32(row["SoLuong"]),
                    DonGia = Convert.ToDecimal(row["DonGia"]),
                    ThanhTien = Convert.ToDecimal(row["ThanhTien"])
                });
            }
            return list;
        }

        public bool Insert(ChiTietHoaDonDTO ct, SqlTransaction? transaction = null)
        {
            string query = @"
                INSERT INTO dbo.ChiTietHoaDon (MaHD, MaDT, SoLuong, DonGia, ThanhTien)
                VALUES (@maHD, @maDT, @soLuong, @donGia, @thanhTien)";
            
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@maHD", SqlDbType.Int) { Value = ct.MaHD },
                new SqlParameter("@maDT", SqlDbType.Int) { Value = ct.MaDT },
                new SqlParameter("@soLuong", SqlDbType.Int) { Value = ct.SoLuong },
                new SqlParameter("@donGia", SqlDbType.Decimal) { Value = ct.DonGia },
                new SqlParameter("@thanhTien", SqlDbType.Decimal) { Value = ct.ThanhTien }
            };

            return DatabaseHelper.ExecuteNonQuery(query, parameters, transaction) > 0;
        }
    }
}
