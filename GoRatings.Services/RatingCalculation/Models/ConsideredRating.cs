using GoRatings.Services.RatingCalculation.Interfaces;

namespace GoRatings.Services.RatingCalculation.Models;

public class ConsideredRating : IConsideredRating
{
    public decimal Rating { get; set; }
    public DateTime CreatedDT { get; set; }
    public bool IsAnonymous { get; set; }

	public decimal CalculateRating(DateTime utcNow, int pastDays)
	{
		if (!(pastDays > 0))
			return 0;

		if (CreatedDT > utcNow)
			return 0;

		decimal rating = 100m;

		rating *= 0.2m * Rating;

		rating *= 1.0m - Math.Min((decimal)(utcNow - CreatedDT).TotalDays, pastDays) / pastDays;

		if (IsAnonymous)
			rating *= 0.1m;

		return rating;
	}
}
