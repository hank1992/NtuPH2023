using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NtuPH2023.Data;
using NtuPH2023.Models;

namespace NtuPH2023.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly NtuPH2023Context _context;

        public NewsController(NtuPH2023Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TblNews.AsNoTracking().OrderByDescending(m => m.CreatedTimestamp).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HtmlContent")] TblNews tblNews)
        {
            _context.Add(tblNews);

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Operation done";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Exception"] = ex.Message;
            }
            return View(tblNews);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblNews == null)
            {
                return NotFound();
            }

            var tblRelatedInfo = await _context.TblNews.FindAsync(id);
            if (tblRelatedInfo == null)
            {
                return NotFound();
            }
            return View(tblRelatedInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Uid,CreatedUser,CreatedTimestamp,LastModifiedUser,LastModifiedTimestamp,HtmlContent")] TblNews tblNews)
        {
            if (id != tblNews.Uid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblNews);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Operation done";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblRelatedInfoExists(tblNews.Uid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["Exception"] = ex.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblNews);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblRelatedInfos == null)
            {
                return NotFound();
            }

            var tblNews = await _context.TblNews
                .FirstOrDefaultAsync(m => m.Uid == id);
            if (tblNews == null)
            {
                return NotFound();
            }

            return View(tblNews);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblNews == null)
            {
                return Problem("Entity set 'NtuPH2023Context.tblNews'  is null.");
            }
            var tblNews = await _context.TblNews.FindAsync(id);
            if (tblNews != null)
            {
                _context.TblNews.Remove(tblNews);
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Operation done";
            }
            catch (Exception ex)
            {
                TempData["Exception"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TblRelatedInfoExists(Guid id)
        {
            return (_context.TblNews?.Any(e => e.Uid == id)).GetValueOrDefault();
        }
    }
}
