using CoreDemo.Models;
using CoreDemo.Services;
using CoreDemo.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class HomeController:Controller
    {
        private readonly ICinemaService _cinemaService;


        /* 数据是从service里来的，构造函数注入的方式来获得service 
        * service需要存到类的本地成员里
        * alt+enter就行
        */
        public HomeController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
  
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "电影院列表";

            return View(await _cinemaService.GetllAllAsync());
        }
        public IActionResult Add()
        {
            ViewBag.Title = "添加电影院";
            return View(new Cinema());
        }
        [HttpPost]
        public async Task<IActionResult> Add(Cinema model)
        {
            if (ModelState.IsValid)
            {
                await _cinemaService.AddAsync(model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int cinemaId)
        {
            var cinema = await _cinemaService.GetByIdAsync(cinemaId);
            return View(cinema);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id,Cinema model)
        {
            if (ModelState.IsValid)
            {
                var exist = await _cinemaService.GetByIdAsync(id);
                if (exist == null)
                {
                    return NotFound();
                }
                exist.Name = model.Name;
                exist.Location = model.Location;
                exist.Capacity = model.Capacity;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _cinemaService.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}
