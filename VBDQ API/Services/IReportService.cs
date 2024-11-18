using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface IReportService
    {
        public Task<(List<TopProductDto>?,Mess)> GetTopSelingProduct();
        public Task<(List<TopProductDto>?, Mess)> GetTopSelingProduct1();

        public Task<(List<DailyRevenueDto>?,Mess)> GetDailyRevenuaAsnyc();
        public Task<(List<MonthRevenueDto>?,Mess)> GetMonthRevenuaAsnyc();
        public Task<(List<YearRevenueDto>?,Mess)> GetYearRevenuaAsnyc();
    }
}
