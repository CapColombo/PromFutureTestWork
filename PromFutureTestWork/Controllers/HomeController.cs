using PromFutureTestWork.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PromFutureTestWork.Controllers
{
    public class HomeController : Controller
    {
        private TerminalSettings settings;
        private readonly IRequestHelper _requestHelper;

        public HomeController(IRequestHelper requestHelper)
        {
            _requestHelper = requestHelper;
        }

        public async Task<IActionResult> Index(int terminalId = 1)
        {
            settings = await _requestHelper.GetSettings();
            var model = new TerminalViewModel { Settings = settings, Rows = GetRows(), TerminalId = terminalId };
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
                SaveRows(row);
            }

            return RedirectToAction("Index", new { terminalId = model.terminal_id } );
        }

        public async Task<IActionResult> GetParameters(int id)
        {
            settings = await _requestHelper.GetSettings();
            var model = settings.items.Where(i => i.id == id).Single();
            return PartialView("ParametersPartial", model);
        }

        private void SaveRows(Table? row)
        {
            int key;
            string json = JsonConvert.SerializeObject(row);

            if (HttpContext.Session.GetInt32("key") != null)
                key = (int)HttpContext.Session.GetInt32("key") + 1;
            else
                key = 0;

            HttpContext.Session.SetString($"{key}", json);
            HttpContext.Session.SetInt32("key", key);
        }

        private List<Table> GetRows()
        {
            List<Table> rows = new List<Table>();
            int? key = HttpContext.Session.GetInt32("key");
            for (int i = 0; i <= key; i++)
            {
                string json = HttpContext.Session.GetString($"{i}");
                Table row = JsonConvert.DeserializeObject<Table>(json);
                rows.Add(row);
            }
            return rows;
        }
    }
}
