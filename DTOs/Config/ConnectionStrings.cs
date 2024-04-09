using System.ComponentModel.DataAnnotations;

namespace HangFireServiceAPI.DTOs.Config
{
    public class ConnectionStrings
    {
        [Required]
        public string DbConnection { get; set; } = null!;

        [Required]
        public string HangFireDbConnection { get; set; } = null!;
    }
}
