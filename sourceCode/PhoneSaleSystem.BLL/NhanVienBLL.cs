using System;
using System.Collections.Generic;
using PhoneSaleSystem.DAL;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.BLL
{
    public class NhanVienBLL
    {
        private readonly NhanVienDAL _nhanVienDAL = new();

        public List<NhanVienDTO> GetAll()
        {
            return _nhanVienDAL.GetAll();
        }

        public NhanVienDTO? GetById(int maNV)
        {
            return _nhanVienDAL.GetById(maNV);
        }

        public NhanVienDTO? GetByMaTK(int maTK)
        {
            return _nhanVienDAL.GetByMaTK(maTK);
        }

        public List<NhanVienDTO> Search(string keyword)
        {
            return _nhanVienDAL.Search(keyword.Trim());
        }

        public bool Add(NhanVienDTO nv, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(nv.HoTen))
            {
                errorMessage = "Họ tên nhân viên không được để trống!";
                return false;
            }

            try
            {
                return _nhanVienDAL.Insert(nv);
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi thêm nhân viên: " + ex.Message;
                return false;
            }
        }

        public bool Update(NhanVienDTO nv, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(nv.HoTen))
            {
                errorMessage = "Họ tên nhân viên không được để trống!";
                return false;
            }

            try
            {
                return _nhanVienDAL.Update(nv);
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi cập nhật nhân viên: " + ex.Message;
                return false;
            }
        }

        public bool Delete(int maNV, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                return _nhanVienDAL.Delete(maNV);
            }
            catch (Exception ex)
            {
                errorMessage = "Không thể xóa nhân viên đã lập hóa đơn hoặc lỗi: " + ex.Message;
                return false;
            }
        }
    }
}
