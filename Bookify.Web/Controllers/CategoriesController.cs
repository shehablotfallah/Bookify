using Bookify.Web.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _context.Categories.AsNoTracking().ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View("Form");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View("Form",model);
            }
            else
            {
                var category = new Category { Name= model.Name };
                _context.Add(category);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            else
            {
                var viewmodel = new CategoryFormViewModel
                {
                    Id = id,
                    Name = category.Name 
                };
                return View("Form", viewmodel);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryFormViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View("Form", model);
            }
            else
            {
                var category = _context.Categories.Find(model.Id);
                if (category is null) 
                {
                    return NotFound();
                }
                else
                {
                    category.Name = model.Name;
                    category.LastUpdatedOn = DateTime.Now;
                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            }
        }  
    }
}