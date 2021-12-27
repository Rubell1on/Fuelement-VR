using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UnityEngine;

public class DBUserAchievements
{
    public static async Task<bool> Receive(int userId, int achievementId, DateTime receiveDate)
    {
        MySqlConnection connection = null;

        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"INSERT INTO {DBTableNames.userAchievements} SET userId = \"{userId}\", achievementId = \"{achievementId}\", received = \"1\", receiveDate = \"{receiveDate.ToString("yyyy-MM-dd")}\";";

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

    public static async Task<List<Achievement>> GetUserAchievements(int userId)
    {
        MySqlConnection connection = null;
        List<Achievement> achievements = new List<Achievement>();

        try
        {
            connection = await SQLConnection.GetConnection();
            string sql = $"SELECT {DBTableNames.userAchievements}.id, {DBTableNames.userAchievements}.achievementId, {DBTableNames.achievements}.title, {DBTableNames.achievements}.description, {DBTableNames.userAchievements}.received, {DBTableNames.userAchievements}.receiveDate FROM {DBTableNames.userAchievements} LEFT JOIN {DBTableNames.achievements} ON achievementId = {DBTableNames.achievements}.id WHERE userId = \"{userId}\";";

            MySqlCommand command = new MySqlCommand(sql, connection);

            await Task.Run(() =>
            {
                MySqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    Achievement achievement = new Achievement();
                    int id = 0;
                    Int32.TryParse(reader[1].ToString(), out id);

                    achievement.id = id;
                    achievement.title = reader[2].ToString();
                    achievement.description = reader[3].ToString();
                    achievement.received = reader[4]?.ToString() == "1" ? true : false;
                    achievement.receiveDate = DateTime.Parse(reader[5].ToString());

                    achievements.Add(achievement);
                }

                reader.Close();
                connection.Close();

                return achievements;
            });

            return achievements;
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка: " + e);
            connection.Close();

            return achievements;
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