using AutoMapper;
using Ecomm_demo.Entities;
using Ecomm_demo.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Ok(customerDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetCustomerWithOrdersAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            // Check if email already exists
            var existingCustomer = await _customerRepository.GetByEmailAsync(createCustomerDto.Email);
            if (existingCustomer != null)
            {
                return BadRequest("Customer with this email already exists");
            }

            var customer = _mapper.Map<Customer>(createCustomerDto);
            var createdCustomer = await _customerRepository.AddAsync(customer);
            var customerDto = _mapper.Map<CustomerDto>(createdCustomer);

            return CreatedAtAction(nameof(GetCustomer), new { id = customerDto.CustomerId }, customerDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CreateCustomerDto updateCustomerDto)
        {
            if (!await _customerRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            var customer = _mapper.Map<Customer>(updateCustomerDto);
            customer.CustomerId = id;
            await _customerRepository.UpdateAsync(customer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var deleted = await _customerRepository.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetCustomerOrders(int id)
        {
            if (!await _customerRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            // This would typically be handled by OrderService, but for simplicity:
            var customer = await _customerRepository.GetCustomerWithOrdersAsync(id);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(customer!.Orders);
            return Ok(orderDtos);
        }
    }
}

