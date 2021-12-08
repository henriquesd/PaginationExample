using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginationExample.Data;
using PaginationExample.Models;
using static PaginationExample.Dtos.Pagination;

namespace PaginationExample.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet("populate-database")]
        public async Task<IActionResult> PopulateDatabase([FromServices]AppDbContext context)
        {
            for (int i = 0; i < 1500; i++)
            {
                var todo = new Todo()
                {
                    Id = i + 1,
                    Done = false,
                    CreatedAt = DateTime.Now,
                    Title = $"Task {i}"
                };

                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpGet("pageNumber/{pageNumber:int}/pageSize/{pageSize:int}")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context, int pageNumber = 1, [FromRoute] int pageSize = 10) // "[FromRoute]" is optional;
        {
            var totalItems = await context.Todos.CountAsync();

            var todos = await context.Todos.AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);

            var pagedResponseDto = new PagedResponseDto<Todo>()
            {
                Data = todos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalItems
            };

            return Ok(pagedResponseDto);
        }
    }
}
