using BookIt.Domain.Entities.Common;

namespace BookIt.Domain.Entities;

public class UserPosition : BaseEntity
{
    public ICollection<UserPositionDetail> UserPositionDetails { get; set; } = [];
}
