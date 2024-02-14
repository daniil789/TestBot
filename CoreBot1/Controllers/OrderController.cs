using CoreBot1.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CoreBot1.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        [HttpGet]
        public IActionResult Index(int orderId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessPayment(PaymentViewModel model)
        {
            // Здесь обработка данных платежа
            // model.CardOwner, model.CardNumber, model.ExpiryDate, model.Cvv
            // Добавьте вашу логику обработки платежа

            return RedirectToAction("PaymentSuccess");
        }

        public IActionResult PaymentSuccess()
        {
            return View();
        }
    }
}
