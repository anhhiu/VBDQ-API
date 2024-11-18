using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService service;
        private readonly MyDbcontext context;

        public ReportController(IReportService service, MyDbcontext context)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("topSelingProduct")]
        public async Task<IActionResult> GetTopSelingProduct()
        {
            var (topproduct, mes) = await service.GetTopSelingProduct();

            if (topproduct == null)
            {
                return NotFound();
            } else if (mes.Error == null)
            {
                return Ok(topproduct);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("topproduct")]
        public async Task<IActionResult> GetTopProduct()
        {
            var (top, mes) = await service.GetTopSelingProduct1();

            if (mes.Error == null)
            {
                return Ok(top);
            }
            else
            {
                return BadRequest(mes.Status);
            }
        }

        [HttpGet("total-revenue")]

        public async Task<IActionResult> GetTotalRevenue()
        {
            var total = context.Transactions.Sum(x => x.TotalAmount);

            return Ok(total);
        }

        [HttpGet("total-average")]

        public async Task<IActionResult> GetTotalAVG()
        {
            var avg = context.Transactions.Average(x => x.TotalAmount);
            return Ok(avg);
        }

        [HttpGet("daily-revenue")]
        public async Task<IActionResult> GetDailyRevenua()
        {
            var (date, mes) = await service.GetDailyRevenuaAsnyc();
            if (mes.Error == null)
            {
                return Ok(date);
            }
            else
            {
                return BadRequest(mes.Status);
            }
            
        }

        [HttpGet("month-revenue")]

        public async Task<IActionResult> GetMonthRevenue()
        {
            var (month, mes) = await service.GetMonthRevenuaAsnyc();
            if (mes.Error == null)
            {
                return Ok(month);
            }
            else
            {
                return BadRequest(mes.Status);
            }
        }

        [HttpGet("year-revenue")]

        public async Task<IActionResult> GetYearRevenue()
        {
            var (year, mes) = await service.GetYearRevenuaAsnyc();
            if(mes.Error == null)
            {
                return Ok(year);
            }
            else
            {
                return BadRequest(mes.Status);
            }
        }

        [HttpGet("total-discount")]
        public async Task<IActionResult> GetTotalDiscount()
        {
            var dis = context.TransactionDetails.Sum(x => x.Discount);
            return Ok(dis);
        }

        [HttpGet("total-discount-product")]

        public async Task<IActionResult> GetTotalDiscountProduct()
        {
            var dis = await context.TransactionDetails.GroupBy(x => x.ProductId)
                                                        .Select(x => new
                                                        {
                                                            productID = x.Key,
                                                            discount = x.Sum(x => x.Discount),
                                                        })
                                                        .OrderByDescending(x => x.discount )
                                                        .ToListAsync();

            return Ok(dis);
        }

        [HttpGet("Product-Category-Supplier")]
        public async Task<IActionResult> GetPCS()
        {
            try
            {
                var pcs = await context.Products.Include(c => c.Category).Include(s => s.Supplier)
                                            .Select(x => new ProductCS
                                            {
                                                Id = x.ProductId,
                                                CategoryName = x.Category!.Name,
                                                SupplierName = x.Supplier!.SupplierName,
                                                ContactName = x.Supplier.ContactName,
                                                Quantity = x.Quantity,
                                                Price = x.ProductPrice,
                                                Adress = x.Supplier.Address,

                                            }).ToListAsync();
                if (pcs.Count > 0)
                {
                    return Ok(pcs);
                }

                return StatusCode(500);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
