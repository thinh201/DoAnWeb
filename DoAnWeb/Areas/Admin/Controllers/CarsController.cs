using AspNetCoreHero.ToastNotification.Abstractions;
using DoAnWeb.Areas.Admin.Models;
using DoAnWeb.Context;
using DoAnWeb.Models;
using DoAnWeb.Models.Authentication;
using DoAnWeb.Ultilities;
using DoAnWeb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [AdminAuthentication]
    public class CarsController : Controller
    {
        private readonly MyDbContext _context;
        private readonly INotyfService _otyfService;
        public CarsController(MyDbContext context, INotyfService notyfService)
        {
            _context = context;
            _otyfService = notyfService;
        }

        // GET
        public IActionResult Index()
        {
            var listCar = _context.Cars.ToList();
            return View(listCar);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult CreateCar()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateCar(CreateCarModel car)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Dữ liệu không hợp lệ");
                return View(car);
            }

            try
            {
                var newCar = new Car()
                {
                    Brand = car.Brand,
                    Model = car.Model,
                    Year = car.Year,
                    Color = car.Color,
                    IsActive = false,
                    PricePerDay = car.PricePerDay,
                    Description = car.Description,
                    Slug = Functions.AliasLink(car.Brand + car.Model)
                };
                _context.Database.BeginTransaction();
                var result = _context.Cars.Add(newCar);
                if (result.State == EntityState.Added)
                {
                    newCar.CarId = result.Entity.CarId;
                }

                _context.SaveChanges();
                if (car.CarImages != null)
                {
                    var imageUrls = UploadImage.UploadMultipleImages(car.CarImages);
                    var carImages = new List<CarImage>();
                    foreach (var imageUrl in imageUrls)
                    {
                        carImages.Add(new CarImage()
                        {
                            CarId = newCar.CarId,
                            ImageUrl = imageUrl
                        });
                    }

                    _context.CarImages.AddRange(carImages);
                    _context.SaveChanges();
                }

                _context.Database.CommitTransaction();
                _otyfService.Success("Thành công");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _context.Database.RollbackTransaction();
                _otyfService.Error("Lỗi mất rối");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult EditCar(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null)
            {
                return View((car, new UpdateCarModel()));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Edit/{id}")]
        public IActionResult EditCar(int id, [Bind(Prefix = "Item2")] UpdateCarModel car)
        {
            try
            {
                var carToUpdate = _context.Cars.Find(id);
                if (carToUpdate != null)
                {
                    carToUpdate.Brand = car.Brand ?? carToUpdate.Brand;
                    carToUpdate.Model = car.Model ?? carToUpdate.Model;
                    carToUpdate.Year = car.Year ?? carToUpdate.Year;
                    carToUpdate.Color = car.Color ?? carToUpdate.Color;
                    carToUpdate.PricePerDay = car.PricePerDay ?? carToUpdate.PricePerDay;
                    carToUpdate.Description = car.Description ?? carToUpdate.Description;
                    carToUpdate.Slug = car.Brand != null && car.Model != null
                        ? Functions.AliasLink(car.Brand + car.Model)
                        : carToUpdate.Slug;
                    _context.Database.BeginTransaction();
                    _context.Cars.Update(carToUpdate);
                    _context.SaveChanges();
                    if (car.CarImages != null)
                    {
                        var imageUrls = UploadImage.UploadMultipleImages(car.CarImages);
                        var carImages = new List<CarImage>();
                        foreach (var imageUrl in imageUrls)
                        {
                            carImages.Add(new CarImage()
                            {
                                CarId = carToUpdate.CarId,
                                ImageUrl = imageUrl
                            });
                        }

                        _context.CarImages.AddRange(carImages);
                        _context.SaveChanges();
                    }

                    _context.Database.CommitTransaction();
                    _otyfService.Success("Thành công");
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _context.Database.RollbackTransaction();
                _otyfService.Success("Lỗi mất rồi");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("Update-Status")]
        public IActionResult UpdateStatus(int idToUpdate)
        {
            var car = _context.Cars.Find(idToUpdate);
            if (car != null)
            {
                car.IsActive = !car.IsActive;
                _context.SaveChanges();
                return new JsonResult(new
                {
                    message = "Success",
                    currentValue = car.IsActive,
                    status = 0
                });
            }

            return new JsonResult(new
            {
                message = "Error",
                status = 1
            });
        }

        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int idToDelete)
        {
            var car = _context.Cars.Find(idToDelete);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
                return new JsonResult(new
                {
                    message = "Success",
                    status = 0
                });
            }

            return new JsonResult(new
            {
                message = "Error",
                status = 1
            });
        }
    }
}