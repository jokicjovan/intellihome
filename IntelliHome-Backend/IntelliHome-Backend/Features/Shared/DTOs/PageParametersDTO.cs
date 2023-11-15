using System.ComponentModel.DataAnnotations;

namespace IntelliHome_Backend.Features.Shared.DTOs
{
    public class PageParametersDTO
    {
        [Range(1, 1000, ErrorMessage = "Page Number temperature should be between 1 and 1000")]
        public int PageNumber { get; set; } = 1;
        [Range(1, 1000, ErrorMessage = "Page Size should be between 1 and 1000")]
        public int PageSize { get; set; } = 10;
    }
}
