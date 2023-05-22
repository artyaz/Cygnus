using System.Collections.Generic;
using System.Threading.Tasks;
using Cygnus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cygnus.Pages
{
    public class ManageUsers : PageModel
    {
        private readonly AppDbContext _db;

        public ManageUsers(AppDbContext db)
        {
            _db = db;
        }

        public List<User> Users { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _db.Users.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostChangeRoleAsync(int userId, string newRole)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.Role = newRole;
            await _db.SaveChangesAsync();

            return RedirectToPage();
        }

        
        public async Task<IActionResult> OnPostDeleteUserAsync(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}