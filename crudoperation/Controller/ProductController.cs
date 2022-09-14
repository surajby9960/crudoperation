using crudoperation.Model;
using crudoperation.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crudoperation.Controller
{
    [Route("big-bazar/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger logger;
        public IProductAsyncRepository productAsyncRepository;
        public ProductController(IConfiguration configuartion, IProductAsyncRepository productAsyncRepository1, ILoggerFactory loggerFactory)
        {
            this.productAsyncRepository = productAsyncRepository1;
            this.logger = loggerFactory.CreateLogger<ProductController>();
        }

        [HttpGet("GetAllProduct")]
        public async Task<ActionResult> GetAllProduct()
        {
            BaseResponse baseResponse = new BaseResponse();

            logger.LogDebug(string.Format($"ProductController-Get:Calling GetAllProduct."));
            var productDetails = await productAsyncRepository.GetAllProduct();
            if (productDetails.Count == 0)
            {
                baseResponse.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponse.StatusMessage = "Data not found";
            }
            else
            {
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = "All Data feached Successfully";
                baseResponse.ResponseData = productDetails;
            }
            return Ok(baseResponse);
        }
    }
}
