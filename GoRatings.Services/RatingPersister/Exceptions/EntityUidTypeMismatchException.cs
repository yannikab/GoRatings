﻿using GoRatings.Services.RatingPersister.Models;

namespace GoRatings.Services.RatingPersister.Exceptions;

public class EntityUidTypeMismatchException : ApplicationException
{
	private readonly Guid entityUid;
	private readonly EntityType entityType;
	
	public EntityUidTypeMismatchException(Guid entityUid, EntityType entityType)
	{
		this.entityUid = entityUid;
		this.entityType = entityType;
	}

	public override string Message
	{
		get { return $"Entity with unique id {entityUid} is not of given type {entityType}."; }
	}
}