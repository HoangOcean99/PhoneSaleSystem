using System;
using System.Collections.Generic;
using PhoneSaleSystem.DAL;
using PhoneSaleSystem.DTO;

namespace PhoneSaleSystem.BLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL _taiKhoanDAL = new();

        public TaiKhoanDTO? Login(string tenDangNhap, string matKhau, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                errorMessage = "Vui lòng nhập tên đăng nhập!";
                return null;
            }

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                errorMessage = "Vui lòng nhập mật khẩu!";
                return null;
            }

            var user = _taiKhoanDAL.Login(tenDangNhap.Trim(), matKhau);
            if (user == null)
            {
                errorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác!";
                return null;
            }

            return user;
        }

        public List<TaiKhoanDTO> GetAll()
        {
            return _taiKhoanDAL.GetAll();
        }

        public bool CreateAccount(string username, string password, string quyen, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
            {
                errorMessage = "Tên đăng nhập phải có ít nhất 3 ký tự!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                errorMessage = "Mật khẩu phải có ít nhất 6 ký tự!";
                return false;
            }

            var tk = new TaiKhoanDTO
            {
                TenDangNhap = username.Trim(),
                MatKhau = password,
                Quyen = quyen
            };

            try
            {
                return _taiKhoanDAL.Insert(tk);
            }
            catch (Exception ex)
            {
                errorMessage = "Tên đăng nhập đã tồn tại hoặc lỗi hệ thống: " + ex.Message;
                return false;
            }
        }

        public bool ChangePassword(int maTK, string oldPass, string newPass, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(newPass) || newPass.Length < 6)
            {
                errorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự!";
                return false;
            }

            return _taiKhoanDAL.UpdatePassword(maTK, newPass);
        }

        public bool DeleteAccount(int maTK, out string errorMessage)
        {
            errorMessage = string.Empty;
            return _taiKhoanDAL.Delete(maTK);
        }
    }
}
