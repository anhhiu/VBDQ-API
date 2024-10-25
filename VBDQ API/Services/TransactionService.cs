using AutoMapper;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
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

        public async Task<(TransactionDto, Mess)> AddTransaction(TransactionPP model)
        {
            try
            {
                if (model == null)
                {
                    return (null, new Mess { Error = "loi roi", Status = "model transaction is null" });
                }

                var transactionDetailList = new List<TransactionDetail>();

                double sumPrice = 0;
                if (model.TransactionDetails != null && model.TransactionDetails.Any())
                {
                    foreach (var item in model.TransactionDetails)
                    {
                        var product = await context.Products.FindAsync(item.ProductId);

                        if (product == null)
                        {
                            return (null, new Mess { Error = "loi roi", Status = "model product is null" });
                        }

                        double priceAfterDiscount = product.ProductPrice - product.Discount;
                        var uninprice = priceAfterDiscount * item.Quantity;

                        if (product.Quantity < item.Quantity)
                        {
                            return (null, new Mess { Error = "loi roi", Status = "so luong hang trong kho khong co du" });
                        }

                        product.Quantity -= item.Quantity;

                        sumPrice += uninprice;

                        var transactionDetail = new TransactionDetail
                        {
                            ProductId = product.ProductId,
                            Quantity = item.Quantity,
                            Discount = product.Discount,
                            UnitPrice = priceAfterDiscount,
                            TotalPrice = uninprice,
                           
                        };

                        transactionDetailList.Add(transactionDetail);
                    }
                }
                else
                {
                    return (null, new Mess { Error = "loi roi", Status = "don hang khong co san pham" });
                }


                var transaction = new Transaction
                {
                    CustomerId = model.CustomerId,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    PaymentMethod = model.PaymentMethod,
                    TotalAmount = sumPrice,
                };

                await context.Transactions.AddAsync(transaction);

                await context.SaveChangesAsync();

                foreach (var transactionDetai in transactionDetailList)
                {
                    transactionDetai.TransactionId = transaction.TransactionId;

                    context.TransactionDetails.Add(transactionDetai);

                }

                await context.SaveChangesAsync();

                var transactionClient = mapper.Map<TransactionDto>(transaction);

                return (transactionClient, new Mess { Error = null, Status = "sucess" });


            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<Mess> DeleteTransaction(int id)
        {
            var transaction = await context.Transactions.FindAsync(id);

            if(transaction == null)
            {
                return new Mess {Error = "loi roi", Status = "Không tìm thấy, chắc là chưa tạo hoặc xóa rồi đấy cụ" };
            }
            context.Transactions.Remove(transaction);

            await context.SaveChangesAsync();

            return new Mess { Error = null, Status = "Xóa thành công kakaka" };
        }

        public async Task<(IEnumerable<TransactionDto>, Mess)> GetAllTransaction()
        {
            try
            {
                var transaction = await context.Transactions
                .OrderByDescending(t => t.TransactionId)
                .Include(t => t.TransactionDetails).ToListAsync();

                if (transaction == null)
                    return (null, new Mess { Error = "loi roi", Status = "khong co ban ghi nao cả" });

                var transactionClient = mapper.Map<IEnumerable<TransactionDto>>(transaction);

                return (transactionClient, new Mess { Error = null, Status = "sucess" });
            }catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message});
            }
        }

        public async Task<(TransactionDto, Mess)> GetTransactionById(int id)
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
            }catch(Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(TransactionDto, Mess)> UpdateTransaction(TransactionPP model, int id)
        {
            try
            {
                if (model == null)
                {
                    return (null, new Mess { Error = "Lỗi rồi", Status = "Lỗi model" });
                }

                var transaction = await context.Transactions
                    .Include(t => t.TransactionDetails)
                    .FirstOrDefaultAsync(t => t.TransactionId == id);

                if (transaction == null)
                {
                    return (null, new Mess { Error = "Lỗi rồi", Status = "Không có giao dịch nào như này" });
                }

                // Đặt lại tổng giá trị
                double sumPrice = 0;

                if (model.TransactionDetails != null && model.TransactionDetails.Any())
                {
                    foreach (var item in model.TransactionDetails)
                    {
                        var product = await context.Products.FindAsync(item.ProductId);

                        if (product == null)
                        {
                            continue; // Bỏ qua nếu không tìm thấy sản phẩm
                        }

                        if (product.Quantity < item.Quantity)
                        {
                            continue; // Bỏ qua nếu tồn kho không đủ
                        }

                        var priceAfterDiscount = product.ProductPrice - product.Discount;
                        var totalPriceForItem = priceAfterDiscount * item.Quantity;

                        product.Quantity -= item.Quantity;

                        var existingTransactionDetail = transaction.TransactionDetails
                            .FirstOrDefault(t => t.ProductId == item.ProductId);

                        if (existingTransactionDetail != null)
                        {
                            existingTransactionDetail.Quantity = item.Quantity;
                            existingTransactionDetail.UnitPrice = priceAfterDiscount;
                            existingTransactionDetail.TotalPrice = totalPriceForItem;
                            existingTransactionDetail.Discount = product.Discount;
                        }
                        else
                        {
                            var newTransactionDetail = new TransactionDetail
                            {
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                UnitPrice = priceAfterDiscount,
                                TotalPrice = totalPriceForItem,
                                Discount = product.Discount,
                            };
                            transaction.TransactionDetails.Add(newTransactionDetail);
                        }
                    }
                }
                else
                {
                    return (null, new Mess { Error = "Lỗi rồi", Status = "Không có bản ghi nào cả" });
                }

                // Tính tổng giá trị từ tất cả các chi tiết giao dịch hiện có
                sumPrice = transaction.TransactionDetails.Sum(td => td.TotalPrice);

                transaction.Address = model.Address;
                transaction.PhoneNumber = model.PhoneNumber;
                transaction.PaymentMethod = model.PaymentMethod;
                transaction.TotalAmount = sumPrice;

                await context.SaveChangesAsync();

                var transactionClient = mapper.Map<TransactionDto>(transaction);

                return (transactionClient, new Mess { Error = null, Status = "Cập nhật thành công" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return (null, new Mess { Error = "Lỗi rồi", Status = "Update transaction thất bại" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "Lỗi rồi", Status = ex.Message });
            }
        }




        //public async Task<(TransactionDto, Mess)> UpdateTransaction(TransactionPP model, int id)
        //{
        //    try
        //    {
        //        if (model == null)
        //        {
        //            return (null, new Mess { Error = "Lỗi rồi", Status = "Lỗi model" });
        //        }

        //        var transaction = await context.Transactions
        //            .Include(t => t.TransactionDetails) // Load existing transaction details
        //            .FirstOrDefaultAsync(t => t.TransactionId == id);

        //        if (transaction == null)
        //        {
        //            return (null, new Mess { Error = "Lỗi rồi", Status = "Không có giao dịch nào như này" });
        //        }

        //        double sumPrice = 0;

        //        if (model.TransactionDetails != null && model.TransactionDetails.Any())
        //        {
        //            foreach (var item in model.TransactionDetails)
        //            {
        //                var product = await context.Products.FindAsync(item.ProductId);
        //                if (product == null)
        //                {
        //                    return (null, new Mess { Error = "Lỗi rồi", Status = "Không tìm thấy sản phẩm" });
        //                }

        //                if (product.Quantity < item.Quantity)
        //                {
        //                    return (null, new Mess { Error = "Lỗi rồi", Status = "Số lượng sản phẩm không đủ" });
        //                }

        //                var priceAfterDiscount = product.ProductPrice - product.Discount;
        //                var totalPriceForItem = priceAfterDiscount * item.Quantity;

        //                product.Quantity -= item.Quantity;

        //                var existingTransactionDetail = transaction.TransactionDetails
        //                    .FirstOrDefault(t => t.ProductId == item.ProductId);

        //                if (existingTransactionDetail != null)
        //                {
        //                    // Cập nhật chi tiết đã có
        //                    existingTransactionDetail.Quantity = item.Quantity;
        //                    existingTransactionDetail.UnitPrice = priceAfterDiscount;
        //                    existingTransactionDetail.TotalPrice = totalPriceForItem;
        //                    existingTransactionDetail.Discount = product.Discount;
        //                }
        //                else
        //                {
        //                    // Thêm chi tiết mới nếu không có
        //                    var newTransactionDetail = new TransactionDetail
        //                    {
        //                        ProductId = item.ProductId,
        //                        Quantity = item.Quantity,
        //                        UnitPrice = priceAfterDiscount,
        //                        TotalPrice = totalPriceForItem,
        //                        Discount = product.Discount,
        //                    };
        //                    transaction.TransactionDetails.Add(newTransactionDetail);
        //                }

        //                sumPrice += totalPriceForItem;
        //            }
        //        }
        //        else
        //        {
        //            return (null, new Mess { Error = "Lỗi rồi", Status = "Không có bản ghi nào cả" });
        //        }

        //        // Cập nhật các thông tin khác của transaction
        //        transaction.Address = model.Address;
        //        transaction.PhoneNumber = model.PhoneNumber;
        //        transaction.PaymentMethod = model.PaymentMethod;
        //        transaction.TotalAmount = sumPrice; // Giả sử có trường tổng giá trị

        //        await context.SaveChangesAsync();

        //        var transactionClient = mapper.Map<TransactionDto>(transaction);

        //        return (transactionClient, new Mess { Error = null, Status = "Cập nhật thành công" });
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return (null, new Mess { Error = "Lỗi rồi", Status = "Update transaction thất bại" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return (null, new Mess { Error = "Lỗi rồi", Status = ex.Message });
        //    }
        //}

    }
}
