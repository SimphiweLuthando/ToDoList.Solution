using Microsoft.AspNetCore.Identity;    
using System.Collections.Generic;

namespace ToDoList.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Item> Items { get; set; }
    }
}