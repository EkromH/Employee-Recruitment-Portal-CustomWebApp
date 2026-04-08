using DeliveryService.Data;
using DeliveryService.Models.Enums;
using DeliveryService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryService.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DeliveryService.Controllers
{
    public class StaffInfosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment env;

        public StaffInfosController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            env = webHostEnvironment;
        }

        // LIST
        public async Task<IActionResult> Index()
        {
            var staff = await _context.staffInfo.ToListAsync();
            return View(staff);
        }
        //[HttpGet]
        //public async Task<IActionResult> Index(StaffStatus? status, DateTime? searchDate)
        //{
        //    var staffQuery = _context.staffInfo.AsQueryable();

        //    if (status.HasValue)
        //    {
        //        staffQuery = staffQuery.Where(s => s.Status == status.Value);
        //    }

        //    if (searchDate.HasValue)
        //    {
        //        staffQuery = staffQuery.Where(s => s.Date.Date == searchDate.Value.Date);
        //    }

        //    ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(StaffStatus)));
        //    return View(await staffQuery.ToListAsync());
        //}

        [HttpGet]
        public async Task<IActionResult> Index(StaffStatus? status, DateTime? searchDate, string searchQuery)
        {
            var staffQuery = _context.staffInfo.AsQueryable();

            // 1. Filter by Status (Enum)
            if (status.HasValue)
            {
                staffQuery = staffQuery.Where(s => s.Status == status.Value);
            }

            // 2. Filter by Date
            if (searchDate.HasValue)
            {
                staffQuery = staffQuery.Where(s => s.Date.Date == searchDate.Value.Date);
            }

            // 3. Search by Name or Passport No
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                staffQuery = staffQuery.Where(s => s.Name.Contains(searchQuery) ||
                                                  s.PassportNo.Contains(searchQuery));
            }

            // Prepare ViewBag for the dropdowns in the View
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(StaffStatus)));

            // Keep the search term in the search box after the page reloads
            ViewData["CurrentFilter"] = searchQuery;

            return View(await staffQuery.ToListAsync());
        }

        //CREATE
        public IActionResult Create()
        {
            ViewBag.StatusList = Enum.GetValues(typeof(StaffStatus));
            return View();
        }

        // POST: Staff/Create=========work
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = "default.png";

                if (model.Photo != null)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string folder = Path.Combine(env.WebRootPath, "AdsImages");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string filePath = Path.Combine(folder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(fileStream);
                    }
                }

                var staff = new StaffInfo
                {
                    Name = model.Name,
                    MobileNumber = model.MobileNumber,
                    PassportNo = model.PassportNo,
                    FathersName = model.FathersName,
                    MothersName = model.MothersName,
                    Date = model.Date,
                    Status = model.Status,
                    Photo = uniqueFileName
                };

                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.StatusList = Enum.GetValues(typeof(StaffStatus));

            return View(model);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var staff = await _context.staffInfo.FindAsync(id);
            if (staff == null) return NotFound();

            var model = new StaffEditViewModel
            {
                Id = staff.Id,
                Name = staff.Name,
                MobileNumber = staff.MobileNumber,
                PassportNo = staff.PassportNo,
                FathersName = staff.FathersName,
                MothersName = staff.MothersName,
                Date = staff.Date,
                Status = staff.Status,
                ExistingPhotoPath = staff.Photo // To show the current image in the view
            };

            ViewBag.StatusList = Enum.GetValues(typeof(StaffStatus));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StaffEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var staff = await _context.staffInfo.FindAsync(id);
                if (staff == null) return NotFound();

                // Update basic properties
                staff.Name = model.Name;
                staff.MobileNumber = model.MobileNumber;
                staff.PassportNo = model.PassportNo;
                staff.FathersName = model.FathersName;
                staff.MothersName = model.MothersName;
                staff.Date = model.Date;
                staff.Status = model.Status;

                // Image Handling Logic
                if (model.Photo != null)
                {
                    // 1. Delete old photo file if it's not the default
                    if (staff.Photo != "default.png")
                    {
                        string oldPath = Path.Combine(env.WebRootPath, "AdsImages", staff.Photo);
                        if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                    }

                    // 2. Upload new photo
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string folder = Path.Combine(env.WebRootPath, "AdsImages");
                    string filePath = Path.Combine(folder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(fileStream);
                    }

                    staff.Photo = uniqueFileName;
                }

                _context.Update(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.StatusList = Enum.GetValues(typeof(StaffStatus));
            return View(model);
        }



        // GET: Staff/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var staff = await _context.staffInfo
                .FirstOrDefaultAsync(m => m.Id == id);

            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staff/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var staff = await _context.staffInfo.FindAsync(id);
            if (staff == null) return NotFound();

            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.staffInfo.FindAsync(id);

            if (staff != null)
            {
                // 1. Delete the physical photo file if it's not the default
                if (staff.Photo != "default.png")
                {
                    string filePath = Path.Combine(env.WebRootPath, "AdsImages", staff.Photo);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // 2. Remove record from Database
                _context.staffInfo.Remove(staff);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}







    //[HttpPost]
    //public async Task<IActionResult> Create(StaffInfo model, IFormFile photoFile)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        if (photoFile != null)
    //        {
    //            string folder = Path.Combine(_env.WebRootPath, "staff");
    //            if (!Directory.Exists(folder))
    //                Directory.CreateDirectory(folder);

    //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
    //            string path = Path.Combine(folder, fileName);

    //            using (var stream = new FileStream(path, FileMode.Create))
    //            {
    //                await photoFile.CopyToAsync(stream);
    //            }

    //            model.Photo = "/staff/" + fileName;
    //        }

    //        _context.Add(model);
    //        await _context.SaveChangesAsync();

    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewBag.StatusList = Enum.GetValues(typeof(StaffStatus));
    //    return View(model);
    ////}
    //[HttpPost]
    //public async Task<IActionResult> Create(StaffCreateViewModel model)
    //{

    //    if (ModelState.IsValid)
    //    {
    //        if (model.Photo != null)
    //        {
    //            string fn = Guid.NewGuid().ToString() + model.Photo.FileName;
    //            string folder = Path.Combine(env.WebRootPath, "staff");

    //            // Ensure directory exists
    //            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);


    //            string imagepath = Path.Combine(folder, fn);
    //            await model.Photo.CopyToAsync(new FileStream(imagepath, FileMode.Create));


    //            StaffInfo staff = new StaffInfo()
    //            {
    //                Name = model.Name,
    //                MobileNumber = model.MobileNumber,
    //                PassportNo = model.PassportNo,
    //                FathersName=model.FathersName ?? "",
    //                MothersName=model.MothersName ?? "",
    //                Date = DateTime.Now,
    //                Status= model.Status,
    //                Photo=fn
    //            };
    //            await _context.staffInfo.AddAsync(staff);
    //            _context.SaveChanges();
    //            TempData["suc"] = "Student Added Successfully";
    //            return RedirectToAction("Index");
    //        }
    //        TempData["msg"] = "Please Select Image";

    //    }
    //    return View(model);
    //}

    

    //    // DELETE
    //    public async Task<IActionResult> Delete(int id)
    //    {
    //        var staff = await _context.staffInfo.FindAsync(id);
    //        if (staff == null) return NotFound();

    //        return View(staff);
    //    }

    //    [HttpPost, ActionName("Delete")]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var staff = await _context.staffInfo.FindAsync(id);
    //        _context.staffInfo.Remove(staff);
    //        await _context.SaveChangesAsync();

    //        return RedirectToAction(nameof(Index));
    //    }
    //}

   