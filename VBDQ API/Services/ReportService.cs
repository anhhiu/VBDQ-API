using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VBDQ_API.Services
{
    public class ReportService : IReportService
    {
        private readonly MyDbcontext context;

        public ReportService(MyDbcontext context)
        {
            this.context = context;
        }

        public async Task<(List<DailyRevenueDto>?, Mess)> GetDailyRevenuaAsnyc()
        {
            try
            {
                var date = await context.Transactions
                            .GroupBy(t => t.TransactionDate.Date)
                            .Select(x => new DailyRevenueDto
                            {
                                Date = x.Key,
                                ToTalPrice = x.Sum(t => t.TotalAmount),
                            })
                            .OrderByDescending(x => x.Date)
                            .ToListAsync();
                if (date == null)
                {
                    return (null, new Mess { Error = "khong co gi", Status = "khong co bat ki giao dich nao" });
                }
                else
                {
                    return (date, new Mess { Error = null, Status = "sucess" });
                }
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(List<MonthRevenueDto>?, Mess)> GetMonthRevenuaAsnyc()
        {
            try
            {
                var year = await context.Transactions.GroupBy(x => new
                {
                    Year = x.TransactionDate.Year,
                    Month = x.TransactionDate.Month,
                })
               .Select(x => new MonthRevenueDto
               {
                   Month = x.Key.Month,
                   Year = x.Key.Year,
                   ToTalPrice = x.Sum(x => x.TotalAmount),
               })
               .OrderByDescending(x => x.Month)
               .ToListAsync();
                if (year == null)
                {
                    return (null, new Mess { Error = "khong co gi", Status = "khong co bat ki giao dich nao" });
                }
                else
                {
                    return (year, new Mess { Error = null, Status = "sucess" });
                }
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        // lên sử dụng  cách truy vấn này, hiệu suất sẽ ok hơn
        public async Task<(List<TopProductDto>?, Mess)> GetTopSelingProduct()
        {
            var topProduct = await context.TransactionDetails
                                    .Include(x => x.Product)
                                    .GroupBy(x => x.ProductId)
                                    .Select(g => new TopProductDto
                                    {
                                        ProductId = g.Key,
                                        ProductName = g.First().Product!.ProductName ?? "null",
                                        TotalSold = g.Sum(x => x.Quantity),
                                        Revenue = g.Sum(x => x.TotalPrice)
                                    })
                                    .OrderByDescending(x => x.TotalSold)
                                    .Take(10)
                                    .ToListAsync();
            return(topProduct, new Mess { Error = null, Status = "sucess" });
        }
        
        public async Task<(List<TopProductDto>?, Mess)> GetTopSelingProduct1()
        {
            try
            {
                var transactionDetails = await context.TransactionDetails.Include(p => p.Product).ToListAsync();


                var topProduct =  transactionDetails
                                   .GroupBy(x => x.ProductId)                                  
                                   .Select(x => new TopProductDto
                                   {
                                       ProductId = x.Key,
                                       ProductName = x.First().Product?.ProductName ?? "null",
                                       TotalSold = x.Sum(p => p.Quantity),
                                       Revenue = x.Sum(p => p.UnitPrice),
                                   })
                                   .OrderByDescending(x => x.TotalSold)
                                   .Take(10)
                                   .ToList();
                if (topProduct != null)
                {
                    return (topProduct, new Mess { Error = null, Status = "sucess" });
                }
                else
                {
                    return (null, new Mess { Error = "khong co", Status = "khong tim thays" });
                }
            }catch(Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(List<YearRevenueDto>?, Mess)> GetYearRevenuaAsnyc()
        {
            try
            {
                var year = await context.Transactions.GroupBy(x => x.TransactionDate.Year)
                                               .Select(x => new YearRevenueDto
                                               {
                                                   Year = x.Key,
                                                   ToTalPrice = x.Sum(x => x.TotalAmount),
                                               })
                                               .OrderByDescending(x => x.ToTalPrice)
                                               .ToListAsync();
                if (year != null)
                {
                    return (year, new Mess { Error = null, Status = "sucess" });
                }
                else
                {
                    return (null, new Mess { Error = "no transactions", Status = "khong co giao dich nao trong nam" });
                }
            }catch(Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }
    }
}
