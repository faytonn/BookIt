﻿using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class Hall : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public int LocationId { get; set; }
    //public bool IsDeleted { get; set; }
    public GeneralLocation? Location { get; set; }
    public ICollection<Seat> Seats { get; set; } = [];
    public ICollection<EventDetail> EventDetails { get; set; } = [];
}
