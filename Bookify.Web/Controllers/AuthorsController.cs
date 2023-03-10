namespace Bookify.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var authors = _context.Authors.AsNoTracking().ToList();
            var viewModel = _mapper.Map<IEnumerable<AuthorViewModel>>(authors);
            return View(viewModel);
        }

        [HttpGet, AjaxOnly]
        public IActionResult Create() 
        {

            return PartialView("_Form");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(AuthorViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var author = _mapper.Map<Author>(model);
                _context.Add(author);
                _context.SaveChanges();

                var viewModel = _mapper.Map<AuthorViewModel>(author);

                return PartialView("_AuthorRow", viewModel);
            }
        }

        [HttpGet,AjaxOnly]
        public IActionResult Edit(int id)
        {
            var author = _context.Authors.Find(id);
            if (author is null)
            {
                return NotFound();
            }
            else
            {
                var viewModel = _mapper.Map<AuthorViewModel>(author);
                return PartialView("_Form", viewModel);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }
            else
            {
                var author = _context.Authors.Find(model.Id);
                if (author is null)
                {
                    return NotFound();
                }
                else
                {
                    author = _mapper.Map(model, author);
                    author.LastUpdatedOn = DateTime.Now;
                    _context.SaveChanges();

                    var viewModel = _mapper.Map<AuthorViewModel>(author);

                    return PartialView("_AuthorRow", viewModel);
                }
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var author = _context.Authors.Find(id);
            if (author is null)
            {
                return NotFound();
            }
            else
            {
                author.IsDeleted = !author.IsDeleted;
                author.LastUpdatedOn = DateTime.Now;
                _context.SaveChanges();

                return Ok(author.LastUpdatedOn.ToString());
            }
        }

        public IActionResult AllowItem(AuthorFormViewModel model)
        {
            var author = _context.Authors.SingleOrDefault(a => a.Name == model.Name);
            var isAllowed = author is null || author.Id.Equals(model.Id);

            return Json(isAllowed);
        }
    }
}
