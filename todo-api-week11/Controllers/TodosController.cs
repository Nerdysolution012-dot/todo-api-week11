using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using todo_api_week11.DTOs;
using todo_api_week11.Models;
using todo_api_week11.Responses;

namespace todo_api_week11.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodosController : ControllerBase
    {
        private static readonly ConcurrentDictionary<int, TodoItem> _todos = new();
        private static int _nextId = 0;

        // GET /api/todos
        [HttpGet]
        public ActionResult<IEnumerable<TodoResponseDto>> GetAll()
        {
            var result = _todos.Values
                .OrderBy(t => t.Id)
                .Select(MapToResponseDto)
                .ToList();
            return Ok(result);
        }

        // GET /api/todos/{id}
        [HttpGet("{id:int}")]
        public ActionResult<TodoResponseDto> GetById(int id)
        {
            if (!_todos.TryGetValue(id, out var todo))
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = $"Todo with id {id} was not found.",
                    Timestamp = DateTime.UtcNow
                });
            }

            return Ok(MapToResponseDto(todo));
        }

        // POST /api/todos
        [HttpPost]
        public ActionResult<TodoResponseDto> Create([FromBody] CreateTodoDto dto)
        {
            var now = DateTime.UtcNow;
            var businessErrors = ValidateBusinessRules(dto.Title, dto.DueDate, dto.Priority, now);
            if (businessErrors.Count > 0)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = businessErrors,
                    Timestamp = now
                });
            }

            int id = Interlocked.Increment(ref _nextId);
            var todo = new TodoItem
            {
                Id = id,
                Title = dto.Title.Trim(),
                Description = dto.Description,
                IsCompleted = false,
                CreatedAt = now,
                DueDate = dto.DueDate,
                Priority = dto.Priority ?? "Medium"
            };

            _todos[id] = todo;

            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, MapToResponseDto(todo));
        }

        // PUT /api/todos/{id}
        [HttpPut("{id:int}")]
        public ActionResult<TodoResponseDto> Update(int id, [FromBody] UpdateTodoDto dto)
        {
            if (!_todos.TryGetValue(id, out var todo))
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = $"Todo with id {id} was not found.",
                    Timestamp = DateTime.UtcNow
                });
            }

            var now = DateTime.UtcNow;
            var businessErrors = ValidateBusinessRules(dto.Title, dto.DueDate, dto.Priority, now);
            if (businessErrors.Count > 0)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Validation failed",
                    Errors = businessErrors,
                    Timestamp = now
                });
            }

            todo.Title = dto.Title.Trim();
            todo.Description = dto.Description;
            todo.IsCompleted = dto.IsCompleted;
            todo.DueDate = dto.DueDate;
            todo.Priority = dto.Priority ?? "Medium";

            return Ok(MapToResponseDto(todo));
        }

        // DELETE /api/todos/{id}
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_todos.TryRemove(id, out _))
            {
                return NotFound(new ErrorResponse
                {
                    StatusCode = 404,
                    Message = $"Todo with id {id} was not found.",
                    Timestamp = DateTime.UtcNow
                });
            }

            return NoContent();
        }

        private static TodoResponseDto MapToResponseDto(TodoItem item) => new()
        {
            Id = item.Id,
            Title = item.Title,
            Description = item.Description,
            IsCompleted = item.IsCompleted,
            CreatedAt = item.CreatedAt,
            DueDate = item.DueDate,
            Priority = item.Priority
        };

        private static Dictionary<string, string[]> ValidateBusinessRules(
            string title, DateTime? dueDate, string? priority, DateTime now)
        {
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(title))
            {
                errors["title"] = new[] { "Title cannot be only whitespace." };
            }

            if (dueDate.HasValue && dueDate.Value < now)
            {
                errors["dueDate"] = new[] { "DueDate cannot be in the past." };
            }

            if (priority != null && priority != "Low" && priority != "Medium" && priority != "High")
            {
                errors["priority"] = new[] { "Priority must be Low, Medium, or High." };
            }

            return errors;
        }
    }
}
