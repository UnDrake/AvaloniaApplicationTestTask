using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaApplicationTestTask.Models;
using AvaloniaApplicationTestTask.Services.Interfaces;

namespace AvaloniaApplicationTestTask.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString = "Server=localhost;Database=TestDB;User=root;Password=andriiseredenko;";

        public async Task<List<string>> LoadComboBoxItems()
        {
            var items = new List<string>();

            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new MySqlCommand("SELECT item_name FROM ComboBoxItems", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(reader.GetString(0));
            }

            return items;
        }

        public async Task SaveData(SettingsModel settings)
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = @"
                INSERT INTO Settings (id, slider_value, numeric_value, checkbox_state, selected_radio, selected_combo)
                VALUES (1, @slider, @numeric_value, @checkbox, @radio, @combo)
                ON DUPLICATE KEY UPDATE 
                    slider_value = @slider,
                    numeric_value = @numeric_value,
                    checkbox_state = @checkbox,
                    selected_radio = @radio,
                    selected_combo = @combo";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@slider", settings.SliderValue);
            cmd.Parameters.AddWithValue("@numeric_value", settings.NumericValue);
            cmd.Parameters.AddWithValue("@checkbox", settings.CheckboxState);
            cmd.Parameters.AddWithValue("@radio", settings.SelectedRadio);
            cmd.Parameters.AddWithValue("@combo", settings.SelectedCombo);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<SettingsModel> LoadData()
        {
            using var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync();

            var query = "SELECT slider_value, numeric_value, checkbox_state, selected_radio, selected_combo FROM Settings WHERE id = 1";
            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            return await reader.ReadAsync()
                ? new SettingsModel
                {
                    SliderValue = reader.GetInt32(0),
                    NumericValue = reader.GetDecimal(1),
                    CheckboxState = reader.GetBoolean(2),
                    SelectedRadio = reader.GetString(3),
                    SelectedCombo = reader.GetString(4)
                }
                : GetDefaultSettings();
        }

        private static SettingsModel GetDefaultSettings() => new()
        {
            SliderValue = 50,
            NumericValue = 0.00m,
            CheckboxState = false,
            SelectedRadio = "A",
            SelectedCombo = "Опція 1"
        };
    }
}
