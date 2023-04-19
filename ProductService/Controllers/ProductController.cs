using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.AsyncDataServices;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public ProductController(IProductRepo productRepo,IMapper mapper,IMessageBusClient messageBusClient)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepo.GetById(id);
            var readProductDto = _mapper.Map<ReadProductDto>(product);
            return Ok(readProductDto);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var product = await _productRepo.GetByName(name);
            var readProductDto = _mapper.Map<ReadProductDto>(product);
            return Ok(readProductDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdateProductDto updateProductDto)
        {
            try
            {
                var product = _mapper.Map<Product>(updateProductDto);
                product.ProductId = id;
                await _productRepo.Update(id,product);
                _productRepo.SaveChanges();
                var readProductDto = _mapper.Map<ReadProductDto>(product);
                return Ok(readProductDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto createProductDto )
        {
            
                var product = _mapper.Map<Product>(createProductDto);
                _productRepo.Create(product);
                _productRepo.SaveChanges();

                var readProductDto = _mapper.Map<ReadProductDto>(product);

            try
            {
                //send async message    
                var productPublishedDto = _mapper.Map<ProductPublishedDto>(readProductDto); 
                productPublishedDto.Event = "Product_Published";
                _messageBusClient.PublishNewProduct(productPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

                return CreatedAtAction(nameof(GetById),new
                {Id=readProductDto.ProductId},readProductDto);
        }
        
        [HttpGet]
        public async Task <ActionResult<IEnumerable<ReadProductDto>>> GetAll()
        {
            var products = await _productRepo.GetAll();
            var listProducts = _mapper.Map<IEnumerable<ReadProductDto>>(products);
            return Ok(listProducts);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {  
             try
            {   
                await _productRepo.Delete(id);
                _productRepo.SaveChanges();
                return Ok(new { message = $"Product with Id ({id}) has been deleted."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }
    }
}