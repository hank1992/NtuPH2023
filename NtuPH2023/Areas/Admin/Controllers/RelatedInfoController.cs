using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NtuPH2023.Data;
using NtuPH2023.Models;

namespace NtuPH2023.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RelatedInfoController : Controller
    {
        private readonly NtuPH2023Context _context;

        public RelatedInfoController(NtuPH2023Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TblRelatedInfos.AsNoTracking().OrderByDescending(m => m.CreatedTimestamp).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HtmlContent")] TblRelatedInfo tblRelatedInfo)
        {
            _context.Add(tblRelatedInfo);

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
            return View(tblRelatedInfo);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblRelatedInfos == null)
            {
                return NotFound();
            }

            var tblRelatedInfo = await _context.TblRelatedInfos.FindAsync(id);
            if (tblRelatedInfo == null)
            {
                return NotFound();
            }
            return View(tblRelatedInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Uid,CreatedUser,CreatedTimestamp,LastModifiedUser,LastModifiedTimestamp,HtmlContent")] TblRelatedInfo tblRelatedInfo)
        {
            if (id != tblRelatedInfo.Uid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblRelatedInfo);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Operation done";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblRelatedInfoExists(tblRelatedInfo.Uid))
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
            return View(tblRelatedInfo);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblRelatedInfos == null)
            {
                return NotFound();
            }

            var tblRelatedInfo = await _context.TblRelatedInfos
                .FirstOrDefaultAsync(m => m.Uid == id);
            if (tblRelatedInfo == null)
            {
                return NotFound();
            }

            return View(tblRelatedInfo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblRelatedInfos == null)
            {
                return Problem("Entity set 'NtuPH2023Context.TblRelatedInfos'  is null.");
            }
            var tblRelatedInfo = await _context.TblRelatedInfos.FindAsync(id);
            if (tblRelatedInfo != null)
            {
                _context.TblRelatedInfos.Remove(tblRelatedInfo);
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
          return (_context.TblRelatedInfos?.Any(e => e.Uid == id)).GetValueOrDefault();
        }
    }
}
