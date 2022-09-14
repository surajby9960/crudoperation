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
    //[Route("api/[controller]")]
    [Route("/big-bazar/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger logger;
        public IOrderAsyncRepository orderAsyncRepository;

        public OrderController(IConfiguration configuartion, IOrderAsyncRepository orderAsyncRepository, ILoggerFactory loggerFactory)
        {
            this.orderAsyncRepository = orderAsyncRepository;
            this.logger = loggerFactory.CreateLogger<OrderController>();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            BaseResponse baseResponse = new BaseResponse();
            logger.LogDebug(string.Format($"OrderController-Get:Calling GetAll."));
            var orderDetails = await orderAsyncRepository.GetAllOrdersAsync();
            if (orderDetails.Count == 0)
            {
                baseResponse.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponse.StatusMessage = "Data not found";
            }
            else
            {
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = "All Data feached Successfully";
                baseResponse.ResponseData = orderDetails;
            }
            return Ok(baseResponse);
        }

        [HttpGet("GetOrderById")]
        public async Task<ActionResult> GetOrderById(int Id)
        {
            BaseResponse baseResponse = new BaseResponse();
            logger.LogDebug(string.Format($"OrderController-OrderByID:Calling OrderByID action with Id{Id}."));

            if (Id == 0)
            {
                var returnmsg = string.Format("Please enter valid Id");
                logger.LogDebug(returnmsg);
                baseResponse.StatusCode = StatusCodes.Status400BadRequest.ToString();
                baseResponse.StatusMessage = returnmsg;
                return Ok(baseResponse);
            }
            var orderList = await orderAsyncRepository.GetOrderById(Id);
            if (orderList == null)
            {
                var retunmsg = string.Format($"Requested Id {Id} is not available.");
                logger.LogDebug(retunmsg);
                baseResponse.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponse.StatusMessage = retunmsg;
                return Ok(baseResponse);
            }
            var rtnmsg = string.Format($"Completed get action with Id {Id}.");
            logger.LogDebug(rtnmsg);
            baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
            baseResponse.StatusMessage = rtnmsg;
            baseResponse.ResponseData = orderList;
            return Ok(baseResponse);
        }

        [HttpPost]
        public async Task<ActionResult> SaveOrder(OrderModel orderModel)
        {
            BaseResponse baseResponse = new BaseResponse();
            logger.LogDebug(String.Format($"OrderController-SaveOrder:Calling By SaveOrder action."));
            if (orderModel != null)
            {
                var Execution = await orderAsyncRepository.SaveOrder(orderModel);
                if (Execution == -1)
                {
                    var returnmsg = string.Format($"Record Is Already saved With ID{orderModel.OrderCode}");
                    logger.LogDebug(returnmsg);
                    baseResponse.StatusCode = StatusCodes.Status409Conflict.ToString();
                    baseResponse.StatusMessage = returnmsg;
                    return Ok(baseResponse);
                }
                else if (Execution >= 1)
                {
                    var rtnmsg = string.Format("Record added successfully..");
                    logger.LogInformation(rtnmsg);
                    logger.LogDebug(string.Format("OrderController-SaveOrder : Completed Adding Order record"));
                    baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                    baseResponse.StatusMessage = rtnmsg;
                    baseResponse.ResponseData = Execution;
                    return Ok(baseResponse);
                }
                else
                {
                    var rtnmsg1 = string.Format("Error while Adding");
                    logger.LogError(rtnmsg1);
                    baseResponse.StatusCode = StatusCodes.Status409Conflict.ToString();
                    baseResponse.StatusMessage = rtnmsg1;
                    return Ok(baseResponse);
                }

            }
            else
            {
                var returnmsg = string.Format("Record added successfully..");
                logger.LogDebug(returnmsg);
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = returnmsg;
                return Ok(baseResponse);
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateOrder(OrderModel orderModel)
        {
            BaseResponse baseResponse = new BaseResponse();
            logger.LogDebug(string.Format("OrderController-UpdateOrder : Calling UpdateOrder action"));
            if (orderModel != null)
            {
                var Execution = await orderAsyncRepository.UpdateOrder(orderModel);
                if (Execution == -1)
                {
                    var returnmsg = string.Format("Record is already exists.");
                    logger.LogDebug(returnmsg);
                    baseResponse.StatusCode = StatusCodes.Status409Conflict.ToString();
                    baseResponse.StatusMessage = returnmsg;
                    return Ok(baseResponse);
                }
                else if (Execution >= 1)
                {
                    var rtnmsg = string.Format("Record updated successfully..");
                    logger.LogDebug(rtnmsg);
                    baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                    baseResponse.StatusMessage = rtnmsg;
                    baseResponse.ResponseData = Execution;
                    return Ok(baseResponse);
                }
                else
                {
                    var rtnmsg = string.Format("Error while updating");
                    logger.LogDebug(rtnmsg);
                    baseResponse.StatusCode = StatusCodes.Status409Conflict.ToString();
                    baseResponse.StatusMessage = rtnmsg;
                    return Ok(baseResponse);
                }
            }
            else
            {
                var rtnmsg = string.Format("Record updated successfully..");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = rtnmsg;
                return Ok(baseResponse);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteOrder(DeleteOrder deleteOrder)
        {
            BaseResponse baseResponse = new BaseResponse();
            logger.LogDebug(string.Format($"OrderController-DeleteOrder : Calling DeleteOrder action with Id {deleteOrder.Id}"));

            var Execution = await orderAsyncRepository.DeleteOrder(deleteOrder);
            if (Execution == 0)
            {
                var retunmsg = string.Format($"Record with Id {deleteOrder.Id} not found");
                logger.LogDebug(retunmsg);
                baseResponse.StatusCode = StatusCodes.Status404NotFound.ToString();
                baseResponse.StatusMessage = retunmsg;
                return Ok(baseResponse);
            }
            else
            {
                var rtnmsg = string.Format($"Record with Id {deleteOrder.Id} is deleted!");
                logger.LogDebug(rtnmsg);
                baseResponse.StatusCode = StatusCodes.Status200OK.ToString();
                baseResponse.StatusMessage = rtnmsg;
                baseResponse.ResponseData = Execution;
                return Ok(baseResponse);
            }
        }
        [HttpGet("GetAllOrderByPagination")]
        public async Task<ActionResult> GetAllOrderByPagination(int pageno, int pagesize)
        {
            BaseResponse responseDetails = new BaseResponse();
            var orderList = await orderAsyncRepository.GetAllOrderByPagination(pageno, pagesize);
            List<OrderModel> orderModels = (List<OrderModel>)orderList.ResponseData1;
            if (orderModels.Count == 0)
            {
                var returnmsg = string.Format("No records are available for orders");
                logger.LogDebug(returnmsg);
                responseDetails.StatusCode = StatusCodes.Status404NotFound.ToString();
                responseDetails.StatusMessage = returnmsg;
                return Ok(responseDetails);
            }
            var rtnmsg = string.Format("All orderList records are fetched successfully.");
            logger.LogDebug(rtnmsg);
            responseDetails.StatusCode = StatusCodes.Status200OK.ToString();
            responseDetails.StatusMessage = rtnmsg;
            responseDetails.ResponseData = orderList;
            return Ok(responseDetails);
        }

        [HttpGet("GetOrderByOrderCode")]
        public async Task<ActionResult> GetOrderByOrderCode(int OrderCode)
        {
            BaseResponse responseDetails = new BaseResponse();
            logger.LogDebug(string.Format($"TransactionOrderController-GetOrderByOrderCode : Calling GetOrderByOrderCode action with OrderCode {OrderCode}."));
            if (OrderCode == 0)
            {
                var returnmsg = string.Format("Please enter valid OrderCode");
                logger.LogDebug(returnmsg);
                responseDetails.StatusCode = StatusCodes.Status400BadRequest.ToString();
                responseDetails.StatusMessage = returnmsg;
                return Ok(responseDetails);
            }

            var orderList = await orderAsyncRepository.GetOrderByOrderCode(OrderCode);
            if (orderList == null)
            {
                var retunmsg = string.Format($"Requested OrderCode {OrderCode} is not available.");
                logger.LogDebug(retunmsg);
                responseDetails.StatusCode = StatusCodes.Status404NotFound.ToString();
                responseDetails.StatusMessage = retunmsg;
                return Ok(responseDetails);
            }
            var rtnmsg = string.Format($"Completed get action with OrderCode {OrderCode}.");
            logger.LogDebug(rtnmsg);
            responseDetails.StatusCode = StatusCodes.Status200OK.ToString();
            responseDetails.StatusMessage = rtnmsg;
            responseDetails.ResponseData = orderList;
            return Ok(responseDetails);
        }
    }
}
