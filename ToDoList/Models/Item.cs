using System.Collections.Generic;
using MySqlConnector;

namespace ToDoList.Models
{
    public class Item
    {
        public string Description { get; set; }
        public int ItemId { get; set; }
    }
}
