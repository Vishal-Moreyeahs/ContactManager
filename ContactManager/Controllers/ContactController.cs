using ContactManager.Interface;
using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Controllers
{
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactController(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _unitOfWork.GetRepository<Contact>().GetAll();
            return View(contacts);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)
        {
            var contactAdd = await _unitOfWork.GetRepository<Contact>().Add(contact);
            await _unitOfWork.Save();
            if(contactAdd)
                return RedirectToAction("Index");
            return View();
        }

       
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Contact());
            else
                return View(await _unitOfWork.GetRepository<Contact>().GetById(id));
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Name,Email,Phone,Address,CreatedOn,UpdatedOn")] Contact contact)
        {
            
                if (contact.Id == 0)
                {
                    var contactAdd = await _unitOfWork.GetRepository<Contact>().Add(contact);
                }
                else
                { 
                    var contactUpdate = await _unitOfWork.GetRepository<Contact>().Upsert(contact);
                }
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _unitOfWork.GetRepository<Contact>().Delete(id);
            await _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
