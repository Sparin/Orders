using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orders.Model.DTO.Order;
using Orders.Model.Entities;
using Orders.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Orders.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orders;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;

        public OrderController(
            IOrderService orders,
            IMapper mapper,
            ILogger<OrderController> logger)
        {
            _orders = orders;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Search orders by query
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/order?page=0&amp;limit=50
        ///
        /// </remarks>
        /// <param name="query">Searching options</param>
        /// <param name="page">Offset. Depends on limit</param>
        /// <param name="limit">Count of orders per request (max 50)</param>
        /// <response code="200">Successful operation</response>
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderDto>))]
        [HttpGet]
        public async Task<IActionResult> SearchOrdersAsync(SearchQuery query, int page = Helpers.DEFAULT_PAGE, int limit = Helpers.MAX_LIMIT_ON_PAGE)
        {
            Helpers.CorrectPageLimitValues(ref page, ref limit);
            _logger.LogInformation($"Searching orders on page {page} with limit {limit}");

            var entities = await _orders.GetOrdersAsync(query, page, limit);

            var result = _mapper.Map<IEnumerable<OrderDto>>(entities);
            _logger.LogInformation($"User received {entities.Count()} orders");
            return Ok(result);
        }

        /// <summary>
        /// Retrieve order by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/order/5
        ///
        /// </remarks>
        /// <param name="id">Identificator of order</param>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Order is not found</response>        
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(OrderDto))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            _logger.LogInformation($"User trying to get order with id {id}");
            var entity = await _orders.FindOrderAsync(id);

            if (entity == default(Order))
            {
                _logger.LogWarning($"User requested not existing order by id {id}");
                return NotFound();
            }

            _logger.LogInformation($"User received order with id {id}");
            var result = _mapper.Map<OrderDto>(entity);
            return Ok(result);
        }

        /// <summary>
        /// Create new order
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/order
        ///     {
        ///         "firstName": "Ivan",
        ///         "middleName": "Ivanovich",
        ///         "lastName": "Ivanov",
        ///         "fruit": "Apple",
        ///         "amount": 50,
        ///         "address": "Saint-Petersburg, Nevskiy prospect 54/1",
        ///         "isCallbackRequired": true,
        ///         "eta": "2019-08-09T10:53:49.100Z"
        ///     }
        ///
        /// </remarks>
        /// <param name="requestDto">Description of future order</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Entity validation is not passed</response>
        [ProducesResponseType(400, Type = typeof(Dictionary<string, string[]>))]
        [ProducesResponseType(200, Type = typeof(OrderDto))]
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody]CreateOrderDto requestDto)
        {
            _logger.LogInformation($"User trying to create new order");
            var entity = _mapper.Map<Order>(requestDto);

            _logger.LogInformation($"Validating new order");

            if (ModelState.IsValid)
                TryValidateModel(entity);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.FormatModelErrors();
                _logger.LogWarning($"New order did not pass entity validation", errors);
                return BadRequest(errors);
            }


            entity = await _orders.CreateOrderAsync(entity);
            _logger.LogInformation($"User created new order with identificator {entity.Id}");

            var result = _mapper.Map<OrderDto>(entity);
            return Ok(result);
        }

        /// <summary>
        /// Update existing order by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/order
        ///     {
        ///         "firstName": "Ivan",
        ///         "middleName": "Ivanovich",
        ///         "lastName": "Ivanov",
        ///         "fruit": "Apple",
        ///         "amount": 50,
        ///         "address": "Saint-Petersburg, Nevskiy prospect 54/1",
        ///         "isCallbackRequired": true,
        ///         "eta": "2019-08-09T10:53:49.100Z"
        ///         }
        ///
        /// </remarks>
        /// <param name="id">Identificator of order</param>
        /// <param name="requestDto">Description of updating order</param>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Entity validation is not passed</response>
        [ProducesResponseType(400, Type = typeof(Dictionary<string, string[]>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(OrderDto))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderAsync(int id, [FromBody]UpdateOrderDto requestDto)
        {
            _logger.LogInformation($"User trying to update existing order with id {id}");
            var entity = await _orders.FindOrderAsync(id);

            if (entity == default(Order))
            {
                _logger.LogWarning($"User requested not existing order");
                return NotFound();
            }

            entity = _mapper.Map(requestDto, entity);

            _logger.LogInformation($"Validating updated order");

            if (ModelState.IsValid)
                TryValidateModel(entity);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.FormatModelErrors();
                _logger.LogWarning($"Updated order did not pass entity validation", errors);
                return BadRequest(errors);
            }

            entity = await _orders.UpdateOrderAsync(entity);
            _logger.LogInformation($"Order with identificator {entity.Id} updated");

            var result = _mapper.Map<OrderDto>(entity);
            return Ok(result);
        }

        /// <summary>
        /// Delete order by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/order/5
        ///
        /// </remarks>
        /// <param name="id">Identificator of order</param>
        /// <response code="204">Successful operation</response>
        /// <response code="404">Order is not found</response>        
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"User trying to delete order with identificator {id}");
            var entity = await _orders.FindOrderAsync(id);

            if (entity == default(Order))
            {
                _logger.LogWarning($"User requested not existing order");
                return NotFound();
            }

            await _orders.DeleteOrderAsync(entity);
            _logger.LogInformation($"Order with identificator {entity.Id} was deleted");

            return NoContent();
        }
    }
}
