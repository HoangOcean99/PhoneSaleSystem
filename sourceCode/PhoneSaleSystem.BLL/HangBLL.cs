using System;
using System.Collections.Generic;
using PhoneSaleSystem.DAL;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.BLL
{
    public class HangBLL
    {
        private readonly HangDAL _hangDAL = new();

        public List<HangDTO> GetAll()
        {
            return _hangDAL.GetAll();
        }

        public bool AddHang(string tenHang, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(tenHang))
            {
                errorMessage = "Tên hãng không được để trống!";
                return false;
            }

            try
            {
                return _hangDAL.Insert(tenHang.Trim());
            }
            catch (Exception ex)
            {
                errorMessage = "Hãng đã tồn tại hoặc lỗi: " + ex.Message;
                return false;
            }
        }

        public bool UpdateHang(HangDTO hang, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(hang.TenHang))
            {
                errorMessage = "Tên hãng không được để trống!";
                return false;
            }

            return _hangDAL.Update(hang);
        }

        public bool DeleteHang(int maHang, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                return _hangDAL.Delete(maHang);
            }
            catch (Exception ex)
            {
                errorMessage = "Không thể xóa hãng đang có sản phẩm điện thoại liên kết: " + ex.Message;
                return false;
            }
        }
    }
}
