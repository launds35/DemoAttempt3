using DemoAttempt3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoAttempt3.Data
{
    internal static class DbHelper
    {
        public static User Authorize(string login, string password)
        {
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    string sql = @"SELECT u.IdUser, u.IdRole, r.Role, u.FullName 
                               FROM Users u JOIN Roles r ON u.IdRole = r.IdRole 
                               WHERE u.Login = @login AND u.Password = @password;";

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(0),
                                IdRole = reader.GetInt32(1),
                                Role = reader.GetString(2),
                                Name = reader.GetString(3)
                            };
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Введен неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public static List<Good> GetGoods()
        {
            var list = new List<Good>();
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    string sql = @"SELECT g.IdGood, g.Article, g.GoodName, 
                                g.UnitOfMeasure, g.Price, g.Supplier, f.Fabric, 
                                c.Category, g.Discount, g.Count, g.Description, 
                                g.Photo
                                FROM Goods g JOIN Fabrics f ON g.IdFabric = f.IdFabric
                                JOIN Categories c ON c.IdCategory = g.IdCategory;";

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Good
                            {
                                Id = reader.GetInt32(0),
                                Article = reader.GetString(1),
                                GoodName = reader.GetString(2),
                                UnitOfMeasure = reader.GetString(3),
                                Price = reader.GetDouble(4),
                                Supplier = reader.GetString(5),
                                Fabric = reader.GetString(6),
                                Category = reader.GetString(7),
                                Discount = reader.GetInt32(8),
                                Count = reader.GetInt32(9),
                                Description = reader.GetString(10),
                                Photo = reader.IsDBNull(11) ? null : reader.GetString(11)
                            });
                        }

                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show($"Ошибка при загрузке списка товаров: {ex}", "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return null;
            }
        }
    }
}
