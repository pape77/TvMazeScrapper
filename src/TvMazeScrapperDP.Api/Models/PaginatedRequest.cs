using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace TvMazeScrapperDP.Api.Models
{
    public class PaginatedRequest
    {
        [FromQuery]
        [Range(1,int.MaxValue, ErrorMessage = "Please enter a valid page number, minimum is 1")]
        public int Page { get; set; }

        [FromQuery]
        [Range(1,100, ErrorMessage = "Please enter a valid page size value between 1 and 100")]
        public int PageSize{ get; set; }
    }
}