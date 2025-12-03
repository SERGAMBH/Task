using webapi.Interfaces;
using webapi.Models;

namespace webapi.Services
{
    public class TodoRepository : ITodoRepository
    {
        private List<TodoItem> _todoList;

        public TodoRepository()
        {
            InitializeData();
        }

        public IEnumerable<TodoItem> All
        {
            get { return _todoList; }
        }
        
        public bool DoesItemExist(string name)
        {
            return _todoList.Any(item => item.Name == name);
        }
        
        public TodoItem Find(string name)
        {
            return _todoList.FirstOrDefault(item => item.Name == name);
        }
        
        public void Insert(TodoItem item)
        {
            _todoList.Add(item);
        }
        
        public void Update(TodoItem item)
        {
            var todoItem = this.Find(item.Name);
            var index = _todoList.IndexOf(todoItem);
            _todoList.RemoveAt(index);
            _todoList.Insert(index, item);
        }
        
        public void Delete(string name)
        {
            _todoList.Remove(this.Find(name));
        }

        private void InitializeData()
        {
            _todoList = new List<TodoItem>();

            var todoItem1 = new TodoItem
            {
                //ID = "6bb8a868-dba1-4f1a-93b7-24ebce87e243",
                Name = "Learn app development",
                //Notes = "Take Microsoft Learn Courses",
                //Done = true
            };

            var todoItem2 = new TodoItem
            {
                //ID = "b94afb54-a1cb-4313-8af3-b7511551b33b",
                Name = "Develop apps",
                //Notes = "Use Visual Studio and Visual Studio Code",
                //Done = false
            };

            var todoItem3 = new TodoItem
            {
                //ID = "ecfa6f80-3671-4911-aabe-63cc442c1ecf",
                Name = "Publish apps",
                //Notes = "All app stores",
                //Done = false,
            };

            _todoList.Add(todoItem1);
            _todoList.Add(todoItem2);
            _todoList.Add(todoItem3);
        }
    }
}


