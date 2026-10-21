using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PhoneSaleSystem.DAL;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.BLL
{
    public class BanHangBLL
    {
        private readonly DienThoaiDAL _dienThoaiDAL = new();
        private readonly HoaDonDAL _hoaDonDAL = new();
        private readonly ChiTietHoaDonDAL _chiTietHoaDonDAL = new();

        public bool ThanhToan(int maNV, int maKH, List<ChiTietHoaDonDTO> gioHang, out int maHDGenerated, out string errorMessage)
        {
            maHDGenerated = 0;
            errorMessage = string.Empty;

            if (maNV <= 0)
            {
                errorMessage = "Nhân viên thực hiện không hợp lệ!";
                return false;
            }

            if (maKH <= 0)
            {
                errorMessage = "Vui lòng chọn khách hàng mua hàng!";
                return false;
            }

            if (gioHang == null || gioHang.Count == 0)
            {
                errorMessage = "Giỏ hàng đang trống, vui lòng chọn sản phẩm!";
                return false;
            }

            // 1. Kiểm tra tồn kho trước khi thực hiện giao dịch
            foreach (var item in gioHang)
            {
                var dt = _dienThoaiDAL.GetById(item.MaDT);
                if (dt == null)
                {
                    errorMessage = $"Sản phẩm '{item.TenDT}' không tồn tại trong hệ thống!";
                    return false;
                }

                if (dt.SoLuong < item.SoLuong)
                {
                    errorMessage = $"Sản phẩm '{dt.TenDT}' chỉ còn {dt.SoLuong} máy trong kho, không đủ đáp ứng số lượng {item.SoLuong}!";
                    return false;
                }
            }

            // 2. Tính tổng tiền
            decimal tongTien = 0;
            foreach (var item in gioHang)
            {
                item.ThanhTien = item.SoLuong * item.DonGia;
                tongTien += item.ThanhTien;
            }

            // 3. Thực hiện giao dịch Database trong Transaction
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // 3.1. Tạo Hóa Đơn
                var hoaDon = new HoaDonDTO
                {
                    MaNV = maNV,
                    MaKH = maKH,
                    NgayLap = DateTime.Now,
                    TongTien = tongTien
                };

                int maHD = _hoaDonDAL.Insert(hoaDon, transaction);
                if (maHD <= 0)
                {
                    transaction.Rollback();
                    errorMessage = "Không thể tạo hóa đơn!";
                    return false;
                }

                // 3.2. Lưu từng Chi Tiết Hóa Đơn và Trừ Tồn Kho
                foreach (var item in gioHang)
                {
                    item.MaHD = maHD;
                    bool insertDetail = _chiTietHoaDonDAL.Insert(item, transaction);
                    if (!insertDetail)
                    {
                        transaction.Rollback();
                        errorMessage = $"Lỗi khi lưu chi tiết hóa đơn cho sản phẩm '{item.TenDT}'!";
                        return false;
                    }

                    // Trừ tồn kho (giảm số lượng)
                    bool updateStock = _dienThoaiDAL.UpdateSoLuong(item.MaDT, -item.SoLuong, transaction);
                    if (!updateStock)
                    {
                        transaction.Rollback();
                        errorMessage = $"Lỗi cập nhật tồn kho cho sản phẩm '{item.TenDT}'!";
                        return false;
                    }
                }

                // 3.3. Commit Transaction thành công
                transaction.Commit();
                maHDGenerated = maHD;
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                errorMessage = "Lỗi khi xử lý thanh toán: " + ex.Message;
                return false;
            }
        }
    }
}
