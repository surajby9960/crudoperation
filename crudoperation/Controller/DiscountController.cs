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
    [Route("/big-bazar/discount")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly ILogger logger;
        public IDiscountAysncRepository discountAysncRepository;
        public DiscountController(IConfiguration configuartion, IDiscountAysncRepository discountAsyncRepository, ILoggerFactory loggerFactory)
        {
            this.discountAysncRepository = discountAsyncRepository;
            this.logger = loggerFactory.CreateLogger<DiscountController>();
        }

        [HttpGet("GetAllDiscount")]
        public async Task<ActionResult> GetAllDiscount()
        {
            BaseResponse baseResponse = new BaseResponse();

            logger.LogDebug(string.Format($"ProductController-Get:Calling GetAllDiscount."));
            var discountDetails = await discountAysncRepository.GetAllDiscount();
            if (discountDetails.Count == 0)
            {
                baseResponse.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponse.StatusMessage = "Data not found";
            }
            else
            {
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = "All Data feached Successfully";
                baseResponse.ResponseData = discountDetails;
            }
            return Ok(baseResponse);
        }
    }
}
