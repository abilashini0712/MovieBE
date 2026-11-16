namespace MovieTicketBookingSystemBE.DTO.DashboardDto
{

    public class DashboardDto
    {
        public int TotalMovies { get; set; }
        public int TotalBookings { get; set; }
        public int TotalUsers { get; set; }

        public decimal TotalRevenue { get; set; }

        public List<double> MonthlyIncome { get; set; } = new();
        public List<double> MonthlyExpenses { get; set; } = new();

        public List<RecentBookingDto> RecentBookings { get; set; } = new();

        public List<TopMovieDto> TopMovies { get; set; } = new();
    }

    public class RecentBookingDto
    {
        public int BookingId { get; set; }
        public string User { get; set; } = "";
        public string MovieName { get; set; } = "";
        public string Seats { get; set; } = "";
        public DateTime Date { get; set; }
    }

    public class TopMovieDto
    {
        public string MovieName { get; set; } = "";
        public int BookingCount { get; set; }
    }

}
