using PromFutureTestWork.Models;
using Microsoft.AspNetCore.Mvc;

namespace PromFutureTestWork.Controllers
{
    public class HomeController : Controller
    {
        static List<Table> rows = new();
        private TerminalSettings settings;
        private readonly IRequestHelper _requestHelper;
        private readonly HttpContext _httpContext;

        public HomeController(IRequestHelper requestHelper, HttpContext context)
        {
            _requestHelper = requestHelper;
            _httpContext = context;
        }

        public async Task<IActionResult> Index(int terminalId)
        {
            settings = await _requestHelper.GetSettings();
            var model = new TerminalViewModel { Settings = settings, Rows = rows };

            model.TerminalId = terminalId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendCommand(TerminalBindingModel model)
        {
            settings = await _requestHelper.GetSettings();

            var response = await _requestHelper.SendCommand(model);

            Table? row;
            if (response != null)
            {
                row = new()
                {
                    Date = response.item.time_created,
                    Command = settings.items.First(i => i.id == response.item.command_id).name,
                    Parameter1 = response.item.parameter1,
                    Parameter2 = response.item.parameter2,
                    Parameter3 = response.item.parameter3,
                    Status = response.item.state_name
                };
                rows.Add(row);
            }

            return RedirectToAction("Index", new { terminalId = model.terminal_id } );
        }

        public async Task<IActionResult> GetParameters(int id)
        {
            settings = await _requestHelper.GetSettings();
            var model = settings.items.Where(i => i.id == id).Single();
            return PartialView("ParametersPartial", model);
        }
    }
}
