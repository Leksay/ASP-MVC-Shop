using BulkiBook.DataAccess.Repository.IRepository;
using BulkiBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private IUnitOfWork _db;

        public CoverTypeController(IUnitOfWork db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var objects = _db.CoverType.GetAll();
            return View(objects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType cover)
        {
            if(!ModelState.IsValid || cover == null)
            {
                ModelState.AddModelError("error", "Wrong Data");
                return View();
            }

            _db.CoverType.Add(cover);
            _db.Save();

            TempData["success"] = "Cover type successfully added";

            return RedirectToAction(nameof(Index));
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var cover = _db.CoverType.GetFirstOrDefault(x => x.Id == id);

            return View(cover);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType cover)
        {
            

            if(cover == null)
            {
                return NotFound();
            }

            _db.CoverType.Update(cover);
            _db.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverType = _db.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        //POST
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverType = _db.CoverType.GetFirstOrDefault(c => c.Id == id);

            if (coverType == null)
            {
                return NotFound();
            }

            _db.CoverType.Remove(coverType);
            _db.Save();

            TempData["success"] = $"Cover Type {coverType.Name} deleted successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
