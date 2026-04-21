using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Data;
using University.ViewModel;

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

            var vm = new ViewModel.StudentDetailsViewModel
            {
                Id = student.Id,
                LastName = student.LastName,
                FirstMidName = student.FirstMidName,
                EnrollmentDate = student.EnrollmentDate
            };

            //kui student on null, siis tagastame NotFound() tulemuse
            if (student == null)
            {
                return NotFound();
            }

            //kui student on leitud, siis tagasme View(vm) tulemuse
            return View(vm);
        }

        //GET: Student/Create
        //see meetod tagastab vaate, kus saab luua uue student'i
        public IActionResult Create()
        {
            return View();
        }
        //POST: Student/Create
        //see meetod salvestab uue student'i andmebaasi
        [HttpPost]
        //see meetod on kaitstud CSRF rünnakute eest
        //see meetod on asünkroonene, mis tähendab, et see meetod ei saa
        //olla samaaegselt mitu korda käivitatud
        public async Task<IActionResult> Create(StudentCreateViewModel vm)
        {
            //kui model on valiidne, siis loome uue student'i ja salvestame selle andmebaasi
            if (ModelState.IsValid)
            {
                
                var student = new Models.Student
                {
                    LastName = vm.LastName,
                    FirstMidName = vm.FirstMidName,
                    EnrollmentDate = vm.EnrollmentDate
                };
                //lisame student'i andmebaasi ja salvestame muudatused
                _context.Add(student);
                //miks kasutame await?
                //kui kasutame await, siis me ootame kuni salvestamine on lõpetatud
                await _context.SaveChangesAsync();
                //pärast salvestamist suuname kasutaja tagasi Indexi vaatesse
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }
    }
}
