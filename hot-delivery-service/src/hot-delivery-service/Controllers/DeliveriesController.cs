using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hot_delivery_service.CommandHandlers;
using hot_delivery_service.Commands;
using hot_delivery_service.Queries;
using Newtonsoft.Json;
using hot_delivery_service.Models;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace hot_delivery_service.Controllers
{
    //апи контроллер
    public class DeliveriesController : Controller
    {
        private IDeliveryCommandHandler _commandHandler;
        private IDeliveryQuery _queryFacade;

        //внедрение зависимостей
        public DeliveriesController(IDeliveryCommandHandler commandHandler, IDeliveryQuery queryFacade)
        {
            _commandHandler = commandHandler;
            _queryFacade = queryFacade;
        }

        //возвращает все доступные доставки в формате json
        [HttpGet("api/available")]
        public string GetAvailableDeliveries()
        {
            var deliveries = _queryFacade.Deliveries.Where(d => d.Status == DeliveySatus.Available).ToList<Delivery>();
            return JsonConvert.SerializeObject(deliveries);
        }
        
        //закрепить доставку за пользователем
        [HttpPost("api/take")]
        public void TakeDelivery(int userId, int deliveryId)
        {
            var delivery = _queryFacade.Deliveries.FirstOrDefault(d => d.Id == deliveryId);
            if (delivery == null)
            {
                //404 код, если доставки с указанным id не найдено
                HttpContext.Response.StatusCode = 404;
                HttpContext.Response.WriteAsync($"Delivery with id {deliveryId} not found").Wait();
            }
            else if (delivery.Status != DeliveySatus.Available)
            {
                //422, если доставка не доступна
                HttpContext.Response.StatusCode = 422;
                HttpContext.Response.WriteAsync($"Delivery with id {deliveryId} is not available").Wait();
            }
            else
            {
                //если все ок - назначем
                AssignToUserCommand command = new AssignToUserCommand() { DeliveryId = delivery.Id, UserId = userId };
                _commandHandler.Handle(command);
            }
        }        
    }
}
