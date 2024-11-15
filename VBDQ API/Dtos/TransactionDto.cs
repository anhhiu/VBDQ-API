using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? TransactionStatus { get; set; }
        public string? PaymentStatus { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }

        public virtual ICollection<TransactionDetailDto> TransactionDetails { get; set; } = new List<TransactionDetailDto>();
    }

    public class TransactionCreate
    {
        
        public int CustomerId { get; set; }
        public string? PaymentMethod { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public double ShippingFree { get; set; }



        public virtual ICollection<TransactionDetailCreate> TransactionDetails { get; set; } = new List<TransactionDetailCreate>();
    }


    public class TransactionUpdate
    {
        public string? TransactionStatus { get; set; }
    }

    public class TransactionUpdate2
    {
        // Trạng thái giao dịch, ví dụ: "đang xử lý", "đã xác nhận", "hoàn tất", "đã hủy"
        public string? TransactionStatus { get; set; }

        // Phương thức thanh toán, ví dụ: "thanh toán khi nhận hàng", "chuyển khoản"
        public string? PaymentMethod { get; set; }

        // Trạng thái thanh toán, ví dụ: "chưa thanh toán", "đã nhận tiền thành công"
        public string? PaymentStatus { get; set; }

        // Địa chỉ giao hàng
        public string? Address { get; set; }

        // Số điện thoại của khách hàng
        public string? PhoneNumber { get; set; }

        // Danh sách chi tiết sản phẩm trong giao dịch (cập nhật số lượng, đơn giá)
        public List<TransactionDetailUpdate>? TransactionDetails { get; set; }

        // Phí vận chuyển hoặc chi phí bổ sung
        public double? ShippingFee { get; set; }

        // Các ghi chú bổ sung cho giao dịch
        public string? Notes { get; set; }
    }


    public class TransactionPP
    {
        public int CustomerId { get; set; }
        public double TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    }

}
