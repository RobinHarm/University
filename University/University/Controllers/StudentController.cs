using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Data;

namespace University.Controllers
{
    public class StudentController : Controller
    {
        private readonly UniversityContext _context;

        public StudentController
            (
                UniversityContext context
            )
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //leiame kõik student'id ja teisendame need StudentIndexViewModel'iks
            //Miks peab kasutama await?
            //kui me kasutame await, siis me ootame kuni päring on lõpetatud
            //ja saame tulemuse, enne kui me jätkame koodi täitmist
            var result = await _context.Students
                .Select(s => new ViewModel.StudentIndexViewModel
            {
                Id = s.Id,
                LastName = s.LastName,
                FirstMidName = s.FirstMidName,
                EnrollmentDate = s.EnrollmentDate
                //miks kasutame ToListAsync()?
                //kui me kasutame ToListAsync(), siis me saame tulemuse listina
            }).ToListAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            //kui id on null, siis tagastame NotFound() tulemuse
            if (id == null)
            {
                return NotFound();
            }

            //leiame student'i Id järgi
            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            //kui student on null, siis tagastame NotFound() tulemuse
            if (student == null)
            {
                return NotFound();
            }
            //kui student on leitud, siis tagasme View(student) tulemuse
            return View(student);
        }
    }
}
