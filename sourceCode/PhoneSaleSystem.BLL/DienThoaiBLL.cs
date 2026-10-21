using System;
using System.Collections.Generic;
using PhoneSaleSystem.DAL;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.BLL
{
    public class DienThoaiBLL
    {
        private readonly DienThoaiDAL _dienThoaiDAL = new();

        public List<DienThoaiDTO> GetAll()
        {
            return _dienThoaiDAL.GetAll();
        }

        public DienThoaiDTO? GetById(int maDT)
        {
            return _dienThoaiDAL.GetById(maDT);
        }

        public List<DienThoaiDTO> Search(string keyword, int? maHang = null)
        {
            return _dienThoaiDAL.Search(keyword.Trim(), maHang);
        }

        public bool Add(DienThoaiDTO dt, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(dt.TenDT))
            {
                errorMessage = "Tên điện thoại không được để trống!";
                return false;
            }

            if (dt.MaHang <= 0)
            {
                errorMessage = "Vui lòng chọn hãng sản xuất hợp lệ!";
                return false;
            }

            if (dt.GiaNhap < 0 || dt.GiaBan < 0)
            {
                errorMessage = "Giá nhập và giá bán không được âm!";
                return false;
            }

            if (dt.GiaBan < dt.GiaNhap)
            {
                errorMessage = "Cảnh báo: Giá bán đang thấp hơn giá nhập!";
            }

            if (dt.SoLuong < 0)
            {
                errorMessage = "Số lượng tồn kho không được âm!";
                return false;
            }

            try
            {
                return _dienThoaiDAL.Insert(dt);
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi thêm điện thoại: " + ex.Message;
                return false;
            }
        }

        public bool Update(DienThoaiDTO dt, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(dt.TenDT))
            {
                errorMessage = "Tên điện thoại không được để trống!";
                return false;
            }

            if (dt.MaHang <= 0)
            {
                errorMessage = "Vui lòng chọn hãng sản xuất hợp lệ!";
                return false;
            }

            if (dt.GiaNhap < 0 || dt.GiaBan < 0)
            {
                errorMessage = "Giá nhập và giá bán không được âm!";
                return false;
            }

            if (dt.SoLuong < 0)
            {
                errorMessage = "Số lượng tồn kho không được âm!";
                return false;
            }

            try
            {
                return _dienThoaiDAL.Update(dt);
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi cập nhật điện thoại: " + ex.Message;
                return false;
            }
        }

        public bool Delete(int maDT, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                return _dienThoaiDAL.Delete(maDT);
            }
            catch (Exception ex)
            {
                errorMessage = "Không thể xóa sản phẩm đã có trong hóa đơn bán hàng: " + ex.Message;
                return false;
            }
        }
    }
}
