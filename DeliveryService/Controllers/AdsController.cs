using DeliveryService.Data;
using DeliveryService.Models;
using DeliveryService.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Controllers
{
    public class AdsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment env;
        public AdsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            env = webHostEnvironment;
        }
        //====================================================================USE FOR ADMIN====================================c
        // GET: Advertisement
        public async Task<IActionResult> Index()
        {
            var ads = await _context.advertisements.ToListAsync();
            return View(ads);
        }

        // GET: Advertisement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ad = await _context.advertisements
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ad == null) return NotFound();

            return View(ad);
        }


        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdvertisementViewModel Adviewmodel)
        {

            if (ModelState.IsValid)
            {
                if (Adviewmodel.Image != null)
                {
                    string fn = Guid.NewGuid().ToString() + Adviewmodel.Image.FileName;
                    string folder = Path.Combine(env.WebRootPath, "AdsImages");
                    string imagepath = Path.Combine(folder, fn);
                    await Adviewmodel.Image.CopyToAsync(new FileStream(imagepath, FileMode.Create));

                    Advertisement ads = new Advertisement()
                    {
                        Title=Adviewmodel.Title,
                        Description=Adviewmodel.Description,
                        Image=fn
                    };
                    await _context.advertisements.AddAsync(ads);
                    _context.SaveChanges();
                    TempData["suc"] = "Student Added Successfully";
                    return RedirectToAction("Index");
                }
                TempData["msg"] = "Please Select Image";

            }
            return View(Adviewmodel);
        }

        // GET: Advertisement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ad = await _context.advertisements.FindAsync(id);
            if (ad == null) return NotFound();

            return View(ad);
        }
        

        // 2. POST: Advertisement/Edit/5
        // This method ONLY handles the "Save Changes" button click.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Advertisement ad, IFormFile? newImage)
        {
            // The ID in the URL must match the ID in the hidden form field
            if (id != ad.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing record from the DB to handle image logic
                    var existingAd = await _context.advertisements.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                    if (existingAd == null) return NotFound();

                    if (newImage != null)
                    {
                        // Delete the old image file to save space
                        string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/AdsImages", existingAd.Image);
                        if (System.IO.File.Exists(oldPath) && existingAd.Image != "default.png")
                        {
                            System.IO.File.Delete(oldPath);
                        }

                        // Upload the new image
                        string uniqueName = Guid.NewGuid().ToString() + "_" + newImage.FileName;
                        string newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/AdsImages", uniqueName);

                        using (var stream = new FileStream(newPath, FileMode.Create))
                        {
                            await newImage.CopyToAsync(stream);
                        }
                        ad.Image = uniqueName;
                    }
                    else
                    {
                        // If no new image is uploaded, keep the old filename
                        ad.Image = existingAd.Image;
                    }

                    _context.Update(ad);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.advertisements.Any(e => e.Id == ad.Id)) return NotFound();
                    else throw;
                }
            }
            return View(ad);
        }
        

        // GET: Advertisement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ad = await _context.advertisements
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ad == null) return NotFound();

            return View(ad);
        }

        // POST: Advertisement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ad = await _context.advertisements.FindAsync(id);
            if (ad != null)
            {
                _context.advertisements.Remove(ad);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}