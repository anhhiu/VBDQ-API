namespace VBDQ_API.Dtos
{
    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public double ToTalPrice { get; set; }
    }

    public class MonthRevenueDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public double ToTalPrice { get; set; }
    }
    public class YearRevenueDto
    {
        public int Year { get; set; }
        public double ToTalPrice { get; set; }
    }

}
