﻿using GoRatings.Api.Exceptions.Rating;
using GoRatings.Api.Interfaces.Rating;
using GoRatings.Api.Interfaces.Services;
using GoRatings.Api.Models.Rating;

namespace GoRatings.Api.Services.RatingCalculation;

public class RatingCalculationService : IRatingCalculationService
{
	public IOverallRating CalculateOverallRating(IEnumerable<IConsideredRating> consideredRatings, int pastDays)
	{
		int count = 0;
		decimal rating = 0m;

		DateTime utcNow = DateTime.UtcNow;

		foreach (var cr in consideredRatings)
		{
			try
            {
                rating += cr.CalculateRating(utcNow, pastDays);
            }
            catch (RatingCalculationException)
            {
				continue;
            }

			count++;
		}

		if (count > 0)
		{
			rating /= count;

			rating /= 20;

			rating = Math.Truncate(100 * rating) / 100;
		}

		return new OverallRating()
		{
			CalculatedDT = DateTime.UtcNow,
			ConsideredRatings = count,
			Rating = rating,
		};
	}

	public async Task<IOverallRating> CalculateOverallRatingAsync(IEnumerable<IConsideredRating> consideredRatings, int pastDays)
	{
		return await Task.Run(() => CalculateOverallRating(consideredRatings, pastDays));
	}
}
