using webapi.Models;

namespace webapi.Interfaces
{
    public interface ITodoRepository
    {
        bool DoesItemExist(string name);
        IEnumerable<TodoItem> All { get; }
        TodoItem Find(string name);
        void Insert(TodoItem item);
        void Update(TodoItem item);
        void Delete(string id);
    }
}
