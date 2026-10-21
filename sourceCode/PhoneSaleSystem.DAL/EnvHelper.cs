using System;
using System.IO;

namespace PhoneSaleSystem.DAL
{
    public static class EnvHelper
    {
        private static bool _loaded = false;

        public static void Load()
        {
            if (_loaded) return;
            _loaded = true;

            string? envPath = FindEnvFile();
            if (envPath == null || !File.Exists(envPath)) return;

            foreach (var line in File.ReadAllLines(envPath))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#")) continue;

                int equalIndex = trimmed.IndexOf('=');
                if (equalIndex <= 0) continue;

                string key = trimmed.Substring(0, equalIndex).Trim();
                string value = trimmed.Substring(equalIndex + 1).Trim();

                // Loại bỏ dấu ngoặc kép hoặc ngoặc đơn nếu có
                if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                    (value.StartsWith("'") && value.EndsWith("'")))
                {
                    value = value.Substring(1, value.Length - 2);
                }

                // Tự động chuẩn hóa dấu gạch chéo nếu người dùng quen tay gõ \\ trong .env
                value = value.Replace(@"\\", @"\");

                Environment.SetEnvironmentVariable(key, value);
            }
        }

        public static string? Get(string key, string? defaultValue = null)
        {
            Load();
            return Environment.GetEnvironmentVariable(key) ?? defaultValue;
        }

        private static string? FindEnvFile()
        {
            // 1. Kiểm tra tại thư mục làm việc hiện tại
            string currentDir = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDir, ".env");
            if (File.Exists(path)) return path;

            // 2. Kiểm tra từ thư mục thực thi (BaseDirectory) và quét ngược lên thư mục cha
            string? dir = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < 6 && !string.IsNullOrEmpty(dir); i++)
            {
                path = Path.Combine(dir, ".env");
                if (File.Exists(path)) return path;

                var parent = Directory.GetParent(dir);
                dir = parent?.FullName;
            }

            return null;
        }
    }
}

