using System;
using System.Collections.Generic;
using PhoneSaleSystem.DAL;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.BLL
{
    public class KhachHangBLL
    {
        private readonly KhachHangDAL _khachHangDAL = new();

        public List<KhachHangDTO> GetAll()
        {
            return _khachHangDAL.GetAll();
        }

        public KhachHangDTO? GetById(int maKH)
        {
            return _khachHangDAL.GetById(maKH);
        }

        public List<KhachHangDTO> Search(string keyword)
        {
            return _khachHangDAL.Search(keyword.Trim());
        }

        public bool Add(KhachHangDTO kh, out string errorMessage)
        {
            return Add(kh, out _, out errorMessage);
        }

        public bool Add(KhachHangDTO kh, out int generatedId, out string errorMessage)
        {
            generatedId = 0;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(kh.HoTen))
            {
                errorMessage = "Họ tên khách hàng không được để trống!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(kh.SDT) || kh.SDT.Length < 9)
            {
                errorMessage = "Số điện thoại không hợp lệ (tối thiểu 9 số)!";
                return false;
            }

            try
            {
                return _khachHangDAL.Insert(kh, out generatedId);
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi thêm khách hàng: " + ex.Message;
                return false;
            }
        }

        public bool Update(KhachHangDTO kh, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(kh.HoTen))
            {
                errorMessage = "Họ tên khách hàng không được để trống!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(kh.SDT) || kh.SDT.Length < 9)
            {
                errorMessage = "Số điện thoại không hợp lệ (tối thiểu 9 số)!";
                return false;
            }

            try
            {
                return _khachHangDAL.Update(kh);
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi cập nhật khách hàng: " + ex.Message;
                return false;
            }
        }

        public bool Delete(int maKH, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                return _khachHangDAL.Delete(maKH);
            }
            catch (Exception ex)
            {
                errorMessage = "Không thể xóa khách hàng đã có lịch sử hóa đơn: " + ex.Message;
                return false;
            }
        }
    }
}
