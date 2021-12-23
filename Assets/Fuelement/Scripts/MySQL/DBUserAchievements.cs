using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UnityEngine;

public class DBUserAchievements
{
    public static async Task<bool> Receive(int userId, int achievementId)
    {
        MySqlConnection connection = null;

        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.userAchievements} SET userId = \"{userId}\", achievementId = \"{achievementId}\", received = \"1\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            bool result = await Task.Run(() =>
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    connection.Close();
                    return true;
                }
            });

            Debug.Log($"Достижение пользователя под id: {userId} добавлено в базу данных.");
            return result;
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка: " + e);
            connection.Close();
            return false;
        }
    }

    public static async Task<bool> AchievementReceived(int userId, int achievementId)
    {
        MySqlConnection connection = null;

        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"SELECT * FROM {DBTableNames.userAchievements} WHERE userId = \"{userId}\" AND achievementId = \"{achievementId}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);
            
            bool result = await Task.Run(() =>
            {
                MySqlDataReader reader = command.ExecuteReader();
                bool hasRows = reader.Read();

                reader.Close();
                connection.Close();

                return hasRows;
            });

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка: " + e);
            connection.Close();

            return false;
        }
    }

    //public static async Task GetAchievements(int userId)
    //{
    //    MySqlConnection connection = null;

    //    try
    //    {
    //        connection = await SQLConnection.GetConnection();
    //        string sql = $"INSERT INTO {DBTableNames.userAchievements} SET userId = \"{userId}\", achievementId = \"{achievementId}\", received = \"1\";";

    //        MySqlCommand command = new MySqlCommand(sql, connection);

    //        bool result = await Task.Run(() =>
    //        {
    //            using (MySqlDataReader reader = command.ExecuteReader())
    //            {
    //                connection.Close();
    //                return true;
    //            }
    //        });

    //        Debug.Log($"Достижение пользователя под id: {userId} добавлено в базу данных.");
    //        return result;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError("Ошибка: " + e);
    //        connection.Close();
    //        return false;
    //    }
    //}
}