using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VBDQ_API.Conmon;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly MyDbcontext context;
        private readonly IMapper mapper;

        public TransactionService(MyDbcontext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<dynamic>?> AddTransactionAsync(TransactionCreate model)
        {

            var response = new ServiceResponse<dynamic>();

            try
            {
                if (model == null)
                {
                    response.Data = new { };
                    response.Message = "input error";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return response;
                }

                var transactionDetailList = new List<TransactionDetail>();
                double totalPrice = 0;
                double shipfree = model.ShippingFree;
                string transactionStatus;

                if (model.TransactionDetails != null && model.TransactionDetails.Any())
                {
                    foreach (var item in model.TransactionDetails)
                    {
                        var product = await context.Products.FindAsync(item.ProductId);

                        if (product == null)
                        {

                            response.Message = "This product is not availble - san pham nay khong co san";
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            return response;
                        }

                        double aftermoney = product.ProductPrice - product.Discount;
                        double unitPrice = item.Quantity * aftermoney;

                        if (item.Quantity > product.Quantity)
                        {

                            response.Message = "trong kho khong du";
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return response;
                        }

                        product.Quantity -= item.Quantity;

                        totalPrice += unitPrice;

                        var transactionDetail = new TransactionDetail
                        {
                            ProductId = product.ProductId,
                            Quantity = item.Quantity,
                            Discount = product.Discount,
                            UnitPrice = aftermoney,
                            TotalPrice = unitPrice,

                        };

                        transactionDetailList.Add(transactionDetail);
                    }
                }
                else
                {
                    response.Data = new { };
                    response.Message = "transaction not sucessfull - order has no product";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return response;
                }

                // Thiết lập transactionStatus và paymentStatus dựa trên paymentMethod

                // string paymentStatus = model.PaymentMethod == StatusTransactions.ChuyenKhoan ? "Đã nhân tiền thành công" : model.PaymentMethod;

                if (model.PaymentMethod!.ToLower().Trim() == StatusTransactions.ChuyenKhoan.ToLower().Trim())
                {
                    transactionStatus = StatusTransactions.DaXacNhan;
                }
                else // Trường hợp "thanh toán khi nhận hàng"
                {
                    transactionStatus = StatusTransactions.DangXuLy;
                }


                var transaction = new Transaction
                {
                    CustomerId = model.CustomerId,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    TotalAmount = totalPrice - shipfree,
                    PaymentStatus = StatusTransactions.ChuaThanhToan,
                    PaymentMethod = model.PaymentMethod,
                    TransactionStatus = transactionStatus,
                    ShippingFee = shipfree,
                    Notes = model.Notes
                };

                context.Transactions.Add(transaction);

                context.SaveChanges();

                foreach (var item in transactionDetailList)
                {
                    item.TransactionId = transaction.TransactionId;

                    context.TransactionDetails.Add(item);
                }

                await context.SaveChangesAsync();

                response.Data = transaction;
                response.Message = "transaction  sucessfull";
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;

            }
            catch (Exception ex)
            {
                response.Data = new { };
                response.Message = "server error" + ex.Message;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return response;
            }

        }

        public async Task<Mess> DeleteTransaction(int id)
        {
            var transaction = await context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return new Mess { Error = "loi roi", Status = "Không tìm thấy id:" + id };
            }
            context.Transactions.Remove(transaction);

            await context.SaveChangesAsync();

            return new Mess { Error = null, Status = "Xóa thành công" };
        }

        public async Task<(IEnumerable<TransactionDto>?, Mess)> GetAllTransaction()
        {
            try
            {
                var transaction = await context.Transactions
                                        .AsNoTracking()
                                        .OrderByDescending(t => t.TransactionId)
                                        .Include(t => t.Customer)
                                        .Include(t => t.TransactionDetails)
                                        .ThenInclude(t => t.Product)
                                        .ToListAsync();

                if (transaction == null)

                    return (null, new Mess { Error = "loi roi", Status = "khong co ban ghi nao cả" });

                var transactionClient = mapper.Map<IEnumerable<TransactionDto>>(transaction);

                return (transactionClient, new Mess { Error = null, Status = "sucess" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(TransactionDto?, Mess)> GetTransactionById(int id)
        {
            try
            {
                var transaction = await context.Transactions.Include(t => t.TransactionDetails).FirstOrDefaultAsync(t => t.TransactionId == id);

                if (transaction == null)
                {
                    return (null, new Mess { Error = "loi roi", Status = $"Khong co giao dich nao co id = {id}" });
                }
                var transactionClient = mapper.Map<TransactionDto>(transaction);

                return (transactionClient, new Mess { Error = null, Status = "SUCESS" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<ServiceResponse<dynamic>?> UpdateTransactionAsync(TransactionUpdate model, int id)
        {
            var response = new ServiceResponse<dynamic>();
            string paymentStatus = StatusTransactions.ChuaThanhToan;
            string transactionStatus = model.TransactionStatus!.ToLower().Trim();
            // kiem tra null
            if (model == null)
            {
                response.Data = new { };
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "input error";
                return response;
            }

            var transaction = await context.Transactions
                                           .Include(t => t.TransactionDetails)
                                           .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                response.Data = new { };
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = "not found";
                return response;
            }

            if (transaction.TransactionStatus == StatusTransactions.HoanThanh.ToLower().Trim())
            {
                response.Data = transaction;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "đơn hàng đã hoàn thành, không thể cập nhật TransactionStatus nữa";
                return response;
            }

            if (transaction.TransactionStatus == StatusTransactions.DaHuy.ToLower().Trim())
            {
                response.Data = transaction;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "đơn hàng đã hủy, không thể cập nhật TransactionStatus ";
                return response;
            }


            if (transactionStatus == StatusTransactions.DangGiao.ToLower().Trim()
                || transactionStatus == StatusTransactions.HoanThanh.ToLower().Trim()
                || transactionStatus == StatusTransactions.DaHuy.ToLower().Trim())
            {
                if (transactionStatus == StatusTransactions.HoanThanh.ToLower().Trim())
                {
                    paymentStatus = StatusTransactions.DaNhanTienThanhCong;

                }

                if (transactionStatus == StatusTransactions.DaHuy.ToLower().ToLower().Trim())
                {
                    paymentStatus = StatusTransactions.DaHuy;
                    transaction.UpdatedAt = DateTime.Now;

                    context.Transactions.Update(transaction);
                    await context.SaveChangesAsync();

                    response.Data = transaction;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Message = " Huy don hang thanh cong";
                }

                // Chỉ cập nhật TransactionStatus nếu PaymentStatus chưa hoàn thành
                if (model.TransactionStatus != null)
                {
                    transaction.TransactionStatus = model.TransactionStatus;
                }

                // Chỉ cập nhật PaymentStatus nếu cần
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    transaction.PaymentStatus = paymentStatus;
                }

                transaction.UpdatedAt = DateTime.Now;

                context.Transactions.Update(transaction);
                context.SaveChanges();

                response.Data = transaction;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "update sucessful";

            }
            else
            {
                response.Data = new { };
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = "Invalid input";
                return response;
            }

            return response;
        }

        //public async Task<(ServiceResponse<dynamic>?, int skip, int limit, int total)> GetTransactionAsync(GridQuery gridQuery)
        //{
        //    var response = new ServiceResponse<dynamic>();
        //    int totalItems = 0;
        //    int skip = 0, limit = 0;

        //    try
        //    {
        //        // Khởi tạo truy vấn
        //        var transactions = context.Transactions.AsNoTracking()
        //                                  .Include(p => p.Customer).Include(t => t.TransactionDetails)
        //                                  .AsQueryable();

        //        // Lọc (Filter)
        //        if (!string.IsNullOrEmpty(gridQuery.Filter))
        //        {
        //            transactions = transactions.Where(p => p.Address!.ToLower().Contains(gridQuery.Filter.ToLower())
        //                                                || p.TransactionStatus!.ToLower().Contains(gridQuery.Filter.ToLower())
        //                                                || p.CustomerId! == int.Parse(gridQuery.Filter)
        //                                                || p.Customer!.CustomerName!.Contains(gridQuery.Filter.ToLower())
        //                                                || p.TransactionId == int.Parse(gridQuery.Filter)
        //                                                || p.PaymentStatus!.ToLower().Contains(gridQuery.Filter.ToLower())
        //                                                || p.PaymentMethod!.ToLower().Contains(gridQuery.Filter.ToLower()));
        //        }

        //        totalItems = await transactions.CountAsync();

        //        // Nếu không nhập `PageNumber` hoặc `PageSize`, trả về toàn bộ dữ liệu
        //        if (gridQuery.PageNumber > 0 && gridQuery.PageSize > 0)
        //        {
        //            skip = gridQuery.PageNumber;
        //            limit = gridQuery.PageSize;

        //            transactions = transactions.Skip((gridQuery.PageNumber - 1) * gridQuery.PageSize).Take(limit);
        //        }

        //        // Sắp xếp (Sort)
        //        if (!string.IsNullOrEmpty(gridQuery.SortColumn) || gridQuery.SortOrder!.ToLower() == "desc" || gridQuery.SortOrder!.ToLower() == "asc")
        //        {
        //            if (gridQuery.SortOrder!.ToLower() == "desc")
        //            {
        //                transactions = transactions.OrderByDescending(p => EF.Property<object>(p, gridQuery.SortColumn!));
        //            }
        //            else
        //            {
        //                transactions = transactions.OrderBy(p => EF.Property<object>(p, gridQuery.SortColumn!));
        //            }
        //        }
        //        else
        //        {
        //            response.Message = "error input - dòng này nhap asc hoặc desc";
        //            response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            return (response, 0, 0, 0);
        //        }

        //        // Thực hiện truy vấn và trả về kết quả
        //        var pagedTransactions = await transactions.ToListAsync();

        //        response.Data = pagedTransactions;
        //        response.Message = "Success";
        //        response.StatusCode = (int)HttpStatusCode.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Data = new { };
        //        response.Message = $"Error: {ex.Message}";
        //        response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return (response, 0, 0, 0);
        //    }

        //    return (response, skip, limit, totalItems);
        //}


        public async Task<(ServiceResponse<dynamic>?, int skip, int limit, int total)> GetTransactionAsync(GridQuery? gridQuery)
        {
            var response = new ServiceResponse<dynamic>();
            int totalItems = 0;
            int skip = 0, limit = 0;

            try
            {
                var transactions = context.Transactions.AsNoTracking()
                                          .Include(p => p.Customer)
                                          .Include(t => t.TransactionDetails)
                                          .AsQueryable();

                // Lọc (Filter)
                if (!string.IsNullOrEmpty(gridQuery.Filter))
                {
                    if (int.TryParse(gridQuery.Filter, out int filterInt))
                    {
                        transactions = transactions.Where(p => p.CustomerId == filterInt || p.TransactionId == filterInt);
                    }

                    transactions = transactions.Where(p =>
                           (p.Address != null && p.Address.ToLower().Contains(gridQuery.Filter.ToLower()))
                        || (p.TransactionStatus != null && p.TransactionStatus.ToLower().Contains(gridQuery.Filter.ToLower()))
                        || (p.Customer != null && p.Customer.CustomerName != null && p.Customer.CustomerName.ToLower().Contains(gridQuery.Filter.ToLower()))
                        || (p.PaymentStatus != null && p.PaymentStatus.ToLower().Contains(gridQuery.Filter.ToLower()))
                        || (p.PaymentMethod != null && p.PaymentMethod.ToLower().Contains(gridQuery.Filter.ToLower()))
                    );
                }

                totalItems = await transactions.CountAsync();

                // Phân trang
                if (gridQuery.PageNumber > 0 && gridQuery.PageSize > 0)
                {
                    skip = gridQuery.PageNumber;
                    limit = gridQuery.PageSize;
                    transactions = transactions.Skip((gridQuery.PageNumber - 1) * gridQuery.PageSize).Take(limit);
                }

                // Sắp xếp (Sort)
                if (!string.IsNullOrEmpty(gridQuery.SortColumn) && !string.IsNullOrEmpty(gridQuery.SortOrder))
                {
                    if (!typeof(Transaction).GetProperties().Any(prop => prop.Name.Equals(gridQuery.SortColumn, StringComparison.OrdinalIgnoreCase)))
                    {
                        response.Message = "Invalid SortColumn.";
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return (response, 0, 0, 0);
                    }

                    transactions = gridQuery.SortOrder.ToLower() == "desc"
                        ? transactions.OrderByDescending(p => EF.Property<object>(p, gridQuery.SortColumn))
                        : transactions.OrderBy(p => EF.Property<object>(p, gridQuery.SortColumn));
                }


                var pagedTransactions = await transactions.ToListAsync();

                if (totalItems == 0)
                {
                    response.Data = pagedTransactions;
                    response.Message = "khong co giao dich nao";
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return (response, 0, 0, 0);
                }

                response.Data = pagedTransactions;
                response.Message = "Success";
                response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {

                response.Data = new { };
                response.Message = $"Error: {ex.Message}";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return (response, 0, 0, 0);
            }

            return (response, skip, limit, totalItems);
        }

    }
}
