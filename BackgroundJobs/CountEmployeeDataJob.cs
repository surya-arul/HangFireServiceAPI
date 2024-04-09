using HangFireServiceAPI.Models.Config;
using HangFireServiceAPI.Repositories;
using Microsoft.Extensions.Options;

namespace HangFireServiceAPI.BackgroundTasks
{
    public interface ICountEmployeeDataJob
    {
        Task CountEmployeeDataAsync(string filePath);
    }

    public class CountEmployeeDataJob : ICountEmployeeDataJob
    {
        private readonly ILogger<CountEmployeeDataJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CountEmployeeDataJob(ILogger<CountEmployeeDataJob> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task CountEmployeeDataAsync(string filePath)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();

                    // Ensure the database table exists before writing the file
                    if (await employeeRepository.IsTableExists())
                    {
                        var employeeCount = await employeeRepository.GetEmployeesCount();

                        var directoryPath = Path.GetDirectoryName(filePath);

                        // Ensure the directory exists before writing the file
                        if (Directory.Exists(directoryPath))
                        {
                            // To append text in file
                            using (StreamWriter writer = File.AppendText(filePath))
                            {
                                await writer.WriteLineAsync($"Current count of employees: {employeeCount} - [{DateTime.Now}]");
                            }

                            // To overwrite text in file
                            //await File.WriteAllTextAsync(filePath, $"Current count of employees: {employeeCount} - [{DateTime.Now}]");
                        }
                        else
                        {
                            _logger.LogWarning($"Directory '{directoryPath}' does not exist. Skipping file writing operation.");
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Table does not exist. Skipping file writing operation.");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
