using System;
using System.Collections.Generic;
using PhoneSaleSystem.DAL;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.BLL
{
    public class ThongKeBLL
    {
        private readonly ThongKeDAL _thongKeDAL = new();

        public List<DoanhThuDTO> GetDoanhThu(DateTime tuNgay, DateTime denNgay)
        {
            return _thongKeDAL.GetDoanhThu(tuNgay.Date, denNgay.Date.AddDays(1).AddTicks(-1));
        }

        public List<SanPhamBanChayDTO> GetTopSanPhamBanChay(int top = 5)
        {
            return _thongKeDAL.GetTopSanPhamBanChay(top);
        }

        public List<SanPhamBanChayDTO> GetTopSanPham(int top = 5)
        {
            return GetTopSanPhamBanChay(top);
        }

        public List<TonKhoDTO> GetBaoCaoTonKho()
        {
            return _thongKeDAL.GetBaoCaoTonKho();
        }

        public (decimal TongDoanhThu, int TongHoaDon, int TongSanPhamDaBan) GetTongQuan(DateTime tuNgay, DateTime denNgay)
        {
            var dsDoanhThu = GetDoanhThu(tuNgay, denNgay);
            decimal tongTien = 0;
            int tongHD = 0;

            foreach (var dt in dsDoanhThu)
            {
                tongTien += dt.DoanhThu;
                tongHD += dt.SoHoaDon;
            }

            var dsTop = GetTopSanPhamBanChay(100);
            int tongSP = 0;
            foreach (var sp in dsTop)
            {
                tongSP += sp.TongDaBan;
            }

            return (tongTien, tongHD, tongSP);
        }
    }
}
