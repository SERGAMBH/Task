using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsDBController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsDBController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItemsControllerDB
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItemsControllerDB/5
        [HttpGet("{name}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(string Name)
        {
            var todoItem = await _context.TodoItems.FindAsync(Name);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItemsControllerDB/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */
        // POST: api/TodoItemsControllerDB
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            try
            {
                if (todoItem.Name == null)
                {
                    return BadRequest(ErrorCode.TodoItemNameAndNotesRequired.ToString());
                }
                //bool itemExists = _todoRepository.DoesItemExist(todoItem.Name);
                //bool TodoItemExists(string name)
                //{
                 //   return _context.TodoItems.Any(e => e.Name == name);
                //}

                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();
                //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
                return CreatedAtAction(nameof(GetTodoItem), new { name = todoItem.Name }, todoItem);
            }
            catch (Exception)
            {
                return BadRequest(ErrorCode.CouldNotCreateItem.ToString());
            }
            //return Ok(todoItem);

        }

        // DELETE: api/TodoItemsControllerDB/5
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteTodoItem(string Name)
        {
            var todoItem = await _context.TodoItems.FindAsync(Name);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /* 
         private bool TodoItemExists(long id)
         {
             return _context.TodoItems.Any(e => e.Id == id);
         }*/

        public enum ErrorCode
        {
            TodoItemNameAndNotesRequired,
            TodoItemIDInUse,
            RecordNotFound,
            CouldNotCreateItem,
            CouldNotUpdateItem,
            CouldNotDeleteItem
        }
    }
}
