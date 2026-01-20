using Fitness.Areas.AdminPanel.ViewModels;
using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Areas.AdminPanel.Controllers
{
    [Authorize(Roles ="SuperAdmin, Admin")]
    [Area("AdminPanel")]
    public class TrainerController : Controller
    {
        private FitnessDbContext _context { get; }
        public TrainerController(FitnessDbContext context)
        {
            _context=context;
        }
        public IActionResult Index()
        {
            return View(_context.trainers.Where(c => !c.IsDeleted));
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM vm) 
        {
            if (!ModelState.IsValid) return View(vm);
            Trainer trainer = new Trainer
            {
                Name=vm.name,
                Description=vm.description,
                Speciality=vm.speciality,
                IsDeleted=false
            };
            if (vm.imageFile!=null) 
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.imageFile.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create)) 
                {
                    await vm.imageFile.CopyToAsync(stream);
                }
                trainer.ImageURL=fileName;
            }
            _context.Add(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id==null) return BadRequest();
            Trainer? trainer = _context.trainers.Where(c => !c.IsDeleted).FirstOrDefault(i => i.Id==id);
            if (trainer==null) return NotFound();
            trainer.IsDeleted=true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id) 
        {
            if (id==null) return BadRequest();
            Trainer? trainer = _context.trainers.Where(c => !c.IsDeleted).FirstOrDefault(i => i.Id==id);
            if (trainer==null) return NotFound();
            UpdateVM vm = new UpdateVM
            {
                id_=trainer.Id,
                name=trainer.Name,
                speciality=trainer.Speciality,
                description=trainer.Description
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateVM vm) 
        {
            if (!ModelState.IsValid) return View(vm);
            Trainer? trainer = _context.trainers.Where(c => !c.IsDeleted).FirstOrDefault(i => i.Id==vm.id_);
            if (trainer==null) return NotFound();
            trainer.Name=vm.name;
            trainer.Speciality=vm.speciality;
            trainer.Description=vm.description;
            if (vm.imageFile!=null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.imageFile.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.imageFile.CopyToAsync(stream);
                }
                trainer.ImageURL=fileName;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
