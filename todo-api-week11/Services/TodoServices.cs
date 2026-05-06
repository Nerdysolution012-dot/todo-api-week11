using todo_api_week11.Model;

namespace todo_api_week11.Services
{
    public class TodoServices
    {
        private readonly List<TodoItem> _todos = new List<TodoItem>
        {
          new TodoItem { Id = 1, Title = "Buy groceries", Description = "Milk, Bread, Eggs", IsCompleted = false, CreatedAt = DateTime.Now, DueDate = DateTime.Now.AddDays(1), Priority = "High" },
          new TodoItem { Id = 2, Title = "Finish project", Description = "Complete the API project", IsCompleted = false, CreatedAt = DateTime.Now, DueDate = DateTime.Now.AddDays(3), Priority = "Medium" },
          new TodoItem { Id = 3, Title = "Call mom", Description = "Check in with family", IsCompleted = true, CreatedAt = DateTime.Now, DueDate = null, Priority = "Low" },
          new TodoItem { Id = 4, Title = "Workout", Description = "Go to the gym for a workout session", IsCompleted = false, CreatedAt = DateTime.Now, DueDate = DateTime.Now.AddDays(2), Priority = "Medium" },
        };


        //Get all todo items    
        public List<TodoItem> GetAll() => _todos;

        public TodoItem GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than zero.");
            }

            var todo = _todos.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                throw new KeyNotFoundException("Todo item not found.");
            }

            return todo;
        }

        public TodoItem Create(TodoItem newTodo)
        {
            if (newTodo == null)
            {
                throw new ArgumentNullException(nameof(newTodo), "New todo item cannot be null.");
            }

            newTodo.Id = _todos.Max(t => t.Id) + 1;
            newTodo.CreatedAt = DateTime.Now;
            _todos.Add(newTodo);
            return newTodo;
        }

        public void Update(int id, TodoItem updatedTodo)
        {
            if (updatedTodo == null)
            {
                throw new ArgumentNullException(nameof(updatedTodo), "Updated todo item cannot be null.");
            }
            var existingTodo = GetById(id);
            existingTodo.Title = updatedTodo.Title;
            existingTodo.Description = updatedTodo.Description;
            existingTodo.IsCompleted = updatedTodo.IsCompleted;
            existingTodo.DueDate = updatedTodo.DueDate;
            existingTodo.Priority = updatedTodo.Priority;
        }

        public void Delete(int id)
        {
            var todo = GetById(id);
            _todos.Remove(todo);
        }
    }

}
