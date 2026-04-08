using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly AppDbContext _context;

        public AdvertisementController(AppDbContext context)
        {
            _context = context;
        }
        //====================================================================USE FOR USER====================================
        // GET: Staff/Details/5

        // GET: Staff/Details/5
        public async Task<IActionResult> DetailsSearch(int id)
        {
            var staff = await _context.staffInfo
                .FirstOrDefaultAsync(m => m.Id == id);

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // User Side - Show All Ads
        public async Task<IActionResult> Index()
        {
            var ads = await _context.advertisements.ToListAsync();
            return View(ads);

        }


        // GET: Staff/Search
        public IActionResult Search()
        {
            return View(new List<StaffInfo>()); // প্রথমে খালি ফলাফল পাঠাচ্ছে
        }
        // POST: Staff/SearchResults
        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View("Search", Enumerable.Empty<StaffInfo>());
            }

            // সার্চ query এর সাথে PassportNo বা MobileNumber মিলানো
            var staffList = await _context.staffInfo
                                .Where(s => s.PassportNo.Contains(query) || s.MobileNumber.Contains(query))
                                .ToListAsync();

            return View("Search", staffList);
        }






        // GET: Staff/Special
        public async Task<IActionResult> Special()
        {
            // শুধুমাত্র Appointed অবস্থায় থাকা স্টাফরা
            var appointedStaff = _context.staffInfo
                                         .Where(s => s.Status == StaffStatus.Appointed)
                                         .ToList();

            return View(appointedStaff);
        }



        // Admin - Details
        public async Task<IActionResult> Details(int id)
        {
            var ad = await _context.advertisements.FindAsync(id);
            if (ad == null) return NotFound();
            return View(ad);
        }

        // Admin - Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Advertisement ad, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                ad.Image = "/images/" + fileName;
            }

            _context.Add(ad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Admin - Edit
        public async Task<IActionResult> Edit(int id)
        {
            var ad = await _context.advertisements.FindAsync(id);
            return View(ad);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Advertisement ad)
        {
            _context.Update(ad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Admin - Delete
        public async Task<IActionResult> Delete(int id)
        {
            var ad = await _context.advertisements.FindAsync(id);
            return View(ad);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ad = await _context.advertisements.FindAsync(id);
            _context.advertisements.Remove(ad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}