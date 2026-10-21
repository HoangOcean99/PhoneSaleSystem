using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PhoneSaleSystem.DAL
{
    public static class DatabaseHelper
    {
        private static string _connectionString = string.Empty;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = LoadConnectionString();
                }
                return _connectionString;
            }
            set => _connectionString = value;
        }

        private static string LoadConnectionString()
        {
            // 1. Đọc từ file .env hoặc biến môi trường
            string? envConn = EnvHelper.Get("DB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(envConn))
            {
                return envConn;
            }

            // 2. Tự ghép từ các biến rời rạc trong .env nếu có
            string? server = EnvHelper.Get("DB_SERVER");
            string? database = EnvHelper.Get("DB_DATABASE", "PhoneSaleDB");
            if (!string.IsNullOrWhiteSpace(server))
            {
                string? user = EnvHelper.Get("DB_USER");
                string? pass = EnvHelper.Get("DB_PASSWORD");
                if (!string.IsNullOrWhiteSpace(user))
                {
                    return $"Server={server};Database={database};User Id={user};Password={pass};TrustServerCertificate=True;";
                }
                return $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate=True;";
            }

            // 3. Fallback mặc định
            return "Server=.\\SQLEXPRESS;Database=PhoneSaleDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[]? parameters = null)
        {
            var dataTable = new DataTable();
            using var conn = GetConnection();
            using var cmd = new SqlCommand(query, conn);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }
            using var adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            return dataTable;
        }

        public static int ExecuteNonQuery(string query, SqlParameter[]? parameters = null, SqlTransaction? transaction = null)
        {
            if (transaction != null)
            {
                using var cmd = new SqlCommand(query, transaction.Connection, transaction);
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteNonQuery();
            }
            else
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new SqlCommand(query, conn);
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteNonQuery();
            }
        }

        public static object? ExecuteScalar(string query, SqlParameter[]? parameters = null, SqlTransaction? transaction = null)
        {
            if (transaction != null)
            {
                using var cmd = new SqlCommand(query, transaction.Connection, transaction);
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteScalar();
            }
            else
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new SqlCommand(query, conn);
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return cmd.ExecuteScalar();
            }
        }
    }
}
