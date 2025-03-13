using AvaloniaApplicationTestTask.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace AvaloniaApplicationTestTask.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<List<string>> LoadComboBoxItems();
        Task SaveData(SettingsModel settings);
        Task<SettingsModel> LoadData();
    }
}
