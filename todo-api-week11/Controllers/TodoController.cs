using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todo_api_week11.DTOs;
using todo_api_week11.Model;
using todo_api_week11.Response;

namespace todo_api_week11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly List<TodoItem> _todos = new List<TodoItem>
        {
          new TodoItem { Id = 1, Title = "Buy groceries", Description = "Milk, Bread, Eggs", IsCompleted = false, CreatedAt = DateTime.Now, DueDate = DateTime.Now.AddDays(1), Priority = "High" },
          new TodoItem { Id = 2, Title = "Finish project", Description = "Complete the API project", IsCompleted = false, CreatedAt = DateTime.Now, DueDate = DateTime.Now.AddDays(3), Priority = "Medium" },
          new TodoItem { Id = 3, Title = "Call mom", Description = "Check in with family", IsCompleted = true, CreatedAt = DateTime.Now, DueDate = null, Priority = "Low" },
          new TodoItem { Id = 4, Title = "Workout", Description = "Go to the gym for a workout session", IsCompleted = false, CreatedAt = DateTime.Now, DueDate = DateTime.Now.AddDays(2), Priority = "Medium" },
        };


        [HttpGet]
        public IActionResult GetAllTods()
        {
            var todos = _todos.Select(todo => new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                DueDate = todo.DueDate,
                Priority = todo.Priority
            }).ToList();

            return Ok(todos);

        }

        [HttpGet("{id}")]
        public IActionResult GetTodoById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ErrorResponse(400, $"Todo id must be greater than 0."));


            }

            var todo = _todos.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return NotFound(new ErrorResponse(404, $"Todo with id {id} was not found."));

            }

            var todoResponse = new TodoResponseDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                DueDate = todo.DueDate,
                Priority = todo.Priority
            };

            return Ok(todoResponse);
        }

        [HttpPost]
        public IActionResult Create(CreateTodoDto createTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newTodo = new TodoItem
            {
                Id = _todos.Max(t => t.Id) + 1,
                Title = createTodoDto.Title,
                Description = createTodoDto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.Now,
                DueDate = createTodoDto.DueDate,
                Priority = createTodoDto.Priority
            };
            _todos.Add(newTodo);
            var todoResponse = new TodoResponseDto
            {
                Id = newTodo.Id,
                Title = newTodo.Title,
                Description = newTodo.Description,
                IsCompleted = newTodo.IsCompleted,
                CreatedAt = newTodo.CreatedAt,
                DueDate = newTodo.DueDate,
                Priority = newTodo.Priority
            };
            return CreatedAtAction(nameof(GetById), new { id = newTodo.Id }, todoResponse);



        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateTodoDto updateTodoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTodo = _todos.FirstOrDefault(t => t.Id == id);
            if (existingTodo != null)
            {
                return NotFound(new ErrorResponse(404, $"Todo with id {id} was not found."));

            }


            existingTodo.Title = updateTodoDto.Title;
            existingTodo.Description = updateTodoDto.Description;
            existingTodo.IsCompleted = updateTodoDto.IsCompleted;
            existingTodo.DueDate = updateTodoDto.DueDate;
            existingTodo.Priority = updateTodoDto.Priority;

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTodo = _todos.FirstOrDefault(t => t.Id == id);

            if (existingTodo != null)
            {


                _todos.Remove(existingTodo);
                return NoContent();
            }

            else
            {
                return NotFound(new ErrorResponse(404, $"Todo with id {id} was not found."));
            }
        }
    }
}