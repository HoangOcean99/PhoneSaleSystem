using System;
using System.Collections.Generic;
using PhoneSaleSystem.DAL;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.BLL
{
    public class HoaDonBLL
    {
        private readonly HoaDonDAL _hoaDonDAL = new();
        private readonly ChiTietHoaDonDAL _chiTietDAL = new();

        public List<HoaDonDTO> GetAll()
        {
            return _hoaDonDAL.GetAll();
        }

        public HoaDonDTO? GetById(int maHD)
        {
            return _hoaDonDAL.GetById(maHD);
        }

        public List<HoaDonDTO> Search(string? keyword = null, DateTime? tuNgay = null, DateTime? denNgay = null)
        {
            return _hoaDonDAL.Search(keyword, tuNgay, denNgay);
        }

        public List<ChiTietHoaDonDTO> GetChiTietHoaDon(int maHD)
        {
            return _chiTietDAL.GetByHoaDonId(maHD);
        }

        public bool Delete(int maHD, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                return _hoaDonDAL.Delete(maHD);
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi xóa hóa đơn: " + ex.Message;
                return false;
            }
        }
    }
}
