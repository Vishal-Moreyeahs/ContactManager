using ContactManager.Interface;
using ContactManager.Models;
using ContactManager.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net;

namespace ContactManager.Controllers
{
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GeocodingService _geocodingService;

        public ContactController(IUnitOfWork unitOfWork, GeocodingService geocodingService)
        {
            _unitOfWork = unitOfWork;
            _geocodingService = geocodingService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var contacts = await _unitOfWork.GetRepository<Contact>().GetAll();
                return View(contacts);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Contact", new { exceptionMessage = ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)
        {
            try
            {
                var contactAdd = await _unitOfWork.GetRepository<Contact>().Add(contact);
                await _unitOfWork.Save();
                if(contactAdd)
                    return RedirectToAction("Index");
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Contact", new { exceptionMessage = ex.Message });
            }
        }

        public async Task<IActionResult> Location(string address)
        {
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    address = "New York, USA"; // Default address
                }
                var (latitude, longitude) = await _geocodingService.GetCoordinatesAsync(address);

                ViewBag.Latitude = latitude;
                ViewBag.Longitude = longitude;
                ViewBag.Address = address;

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Contact", new { exceptionMessage = ex.Message });
            }
            
        }

        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            try
            {
                if (id == 0)
                    return View(new Contact());
                else
                    return View(await _unitOfWork.GetRepository<Contact>().GetById(id));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Contact", new { exceptionMessage = ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Contact contact)
        {
            try
            {
                if (contact.Id == 0)
                {
                    var contactAdd = await _unitOfWork.GetRepository<Contact>().Add(contact);
                }
                else
                {
                    contact.UpdatedOn = DateTime.Now;
                    var contactUpdate = await _unitOfWork.GetRepository<Contact>().Upsert(contact);
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
                
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your application's logging strategy
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Contact", new { exceptionMessage = ex.Message });

                // You might log ex.Message or handle it differently based on your logging strategy
            }

        }

        public IActionResult Error(string exceptionMessage)
        {
            ViewData["ExceptionMessage"] = exceptionMessage;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _unitOfWork.GetRepository<Contact>().Delete(id);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Error", "Contact", new { exceptionMessage = ex.Message });
            }
        }
    }
}
