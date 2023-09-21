using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NtuPH2023.Data;
using NtuPH2023.Models;

namespace NtuPH2023.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class CommentController : Controller
    {
        private readonly NtuPH2023Context _context;

        public CommentController(NtuPH2023Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TblComments.AsNoTracking().OrderByDescending(m => m.CreatedTimestamp).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HtmlContent")] TblComment tblComment)
        {
            _context.Add(tblComment);

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
            return View(tblComment);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblComments == null)
            {
                return NotFound();
            }

            var tblComment = await _context.TblComments.FindAsync(id);
            if (tblComment == null)
            {
                return NotFound();
            }
            return View(tblComment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Uid,CreatedUser,CreatedTimestamp,LastModifiedUser,LastModifiedTimestamp,HtmlContent")] TblComment tblComment)
        {
            if (id != tblComment.Uid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblComment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Operation done";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCommentExists(tblComment.Uid))
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
            return View(tblComment);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblComments == null)
            {
                return NotFound();
            }

            var TblComment = await _context.TblComments
                .FirstOrDefaultAsync(m => m.Uid == id);
            if (TblComment == null)
            {
                return NotFound();
            }

            return View(TblComment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblComments == null)
            {
                return Problem("Entity set 'NtuPH2023Context.TblComments'  is null.");
            }
            var tblComment = await _context.TblComments.FindAsync(id);
            if (tblComment != null)
            {
                _context.TblComments.Remove(tblComment);
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

        private bool TblCommentExists(Guid id)
        {
            return (_context.TblComments?.Any(e => e.Uid == id)).GetValueOrDefault();
        }
    }
}
