using System.ComponentModel.DataAnnotations;

namespace HangFireServiceAPI.Models.Config
{
    public class HangFireServiceSettings
    {
        [Required]
        public string CronExpression { get; set; } = null!;

        [Required]
        public string FilePath { get; set; } = null!;
    }
}
