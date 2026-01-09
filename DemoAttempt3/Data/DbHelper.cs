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
                MessageBox.Show("Введен неверный логин или пароль", "Ошибка авторизации", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public static int GetMaxId()
        {
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    string sql = @"SELECT MAX(IdGood) FROM Goods;";

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetInt32(0);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при получении максимального ID", "Ошибка работы с БД", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return -1;
        }

        public static bool AddGood(Good good)
        {
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    string sql = @"INSERT INTO Goods 
                    (IdGood, Article, GoodName, UnitOfMeasure, 
                    Price, Supplier, IdFabric, IdCategory, Discount, 
                    Count, Description, Photo) 
                    VALUES (@IdGood, @Article, @GoodName, @UnitOfMeasure, 
                    @Price, @Supplier, @IdFabric, @IdCategory, @Discount, @Count, 
                    @Description, @Photo)";

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@IdGood", good.Id);
                    cmd.Parameters.AddWithValue("@Article", good.Article);
                    cmd.Parameters.AddWithValue("@GoodName", good.GoodName);
                    cmd.Parameters.AddWithValue("@UnitOfMeasure", good.UnitOfMeasure);
                    cmd.Parameters.AddWithValue("@Price", good.Price);
                    cmd.Parameters.AddWithValue("@Supplier", good.Supplier);
                    cmd.Parameters.AddWithValue("@IdFabric", good.IdFabric);
                    cmd.Parameters.AddWithValue("@IdCategory", good.IdCategory);
                    cmd.Parameters.AddWithValue("@Discount", good.Discount);
                    cmd.Parameters.AddWithValue("@Count", good.Count);
                    cmd.Parameters.AddWithValue("@Description", good.Description);
                    cmd.Parameters.AddWithValue("@Photo", good.Photo);

                    int result = cmd.ExecuteNonQuery();
                    
                    if (result > 0)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                MessageBox.Show($"Ошибка при добавлении информации о товаре (NewID: {good.Id})",
                    "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        public static bool UpdateGood(Good good)
        {
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    string sql = @"UPDATE Goods SET
                    Article = @Article, GoodName = @GoodName, UnitOfMeasure = @UnitOfMeasure, 
                    Price = @Price, Supplier = @Supplier, IdFabric = @IdFabric, 
                    IdCategory = @IdCategory, Discount = @Discount, 
                    Count = @Count, Description = @Description, Photo = @Photo
                    WHERE IdGood = @IdGood";

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@IdGood", good.Id);
                    cmd.Parameters.AddWithValue("@Article", good.Article);
                    cmd.Parameters.AddWithValue("@GoodName", good.GoodName);
                    cmd.Parameters.AddWithValue("@UnitOfMeasure", good.UnitOfMeasure);
                    cmd.Parameters.AddWithValue("@Price", good.Price);
                    cmd.Parameters.AddWithValue("@Supplier", good.Supplier);
                    cmd.Parameters.AddWithValue("@IdFabric", good.IdFabric);
                    cmd.Parameters.AddWithValue("@IdCategory", good.IdCategory);
                    cmd.Parameters.AddWithValue("@Discount", good.Discount);
                    cmd.Parameters.AddWithValue("@Count", good.Count);
                    cmd.Parameters.AddWithValue("@Description", good.Description);
                    cmd.Parameters.AddWithValue("@Photo", good.Photo);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                MessageBox.Show($"Ошибка при обновлении информации о товаре (ID: {good.Id})",
                    "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        public static bool DeleteGood(int id)
        {
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    string sql = @"DELETE Goods WHERE IdGood = @IdGood;";

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@IdGood", id);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                MessageBox.Show($"Ошибка при удалении информации о товаре (ID: {id})",
                    "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }
        public static List<ComboBoxItems> GetFabrics()
        {
            var list = new List<ComboBoxItems>();
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    string sql = @"SELECT IdFabric, Fabric FROM Fabrics;";

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }

                    return list;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки выпадающего списка", 
                    "Ошибка работы с базой данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }

        public static List<ComboBoxItems> GetCategories()
        {
            var list = new List<ComboBoxItems>();
            try
            {
                using (SqlConnection conn = Db.GetConnection())
                {
                    string sql = @"SELECT IdCategory, Category FROM Categories;";

                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ComboBoxItems
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }

                    return list;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки выпадающего списка", 
                    "Ошибка работы с базой данных", MessageBoxButton.OK, MessageBoxImage.Error);
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
                                g.UnitOfMeasure, g.Price, g.Supplier, g.IdFabric, f.Fabric, 
                                g.IdCategory, c.Category, g.Discount, g.Count, g.Description, 
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
                                IdFabric = reader.GetInt32(6),
                                Fabric = reader.GetString(7),
                                IdCategory = reader.GetInt32(8),
                                Category = reader.GetString(9),
                                Discount = reader.GetInt32(10),
                                Count = reader.GetInt32(11),
                                Description = reader.GetString(12),
                                Photo = reader.IsDBNull(13) ? null : reader.GetString(13)
                            });
                        }

                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show($"Ошибка при загрузке списка товаров: {ex}", 
                        "Ошибка работы с БД", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return null;
            }
        }
    }
}
