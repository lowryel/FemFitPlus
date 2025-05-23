using System;
using FemFitPlus.Models;
using Microsoft.AspNetCore.Mvc;

namespace FemFitPlus.Services.Filters;

public class ProfileFilter
{
    [FromQuery]
    public string? Id { get; set; }
    [FromQuery]
    public string? Name { get; set; }
    [FromQuery]
    public string? Email { get; set; }
    [FromQuery]
    public bool? IsActive { get; set; }

    [FromQuery]
    public DateTime? StartDate { get; set; }
    [FromQuery]
    public DateTime? EndDate { get; set; }
    [FromQuery]
    public string? UserId { get; set; }

    [FromQuery(Name = "_page")]
    public int Page { get; set; }

    [FromQuery(Name = "_size")]
    public int Size { get; set; }

    public int Skip() { return (Page - 1) * Size; }

    public IQueryable<Profile> BuildQuery(IQueryable<Profile> query)
    {
        if (Size == 0)
            Size = 10;
        if (Page == 0)
            Page = 1;
        if (!string.IsNullOrEmpty(Email))
            query = query.Where(q => q.Femfituser!.Email!.Contains(Email));

        if (!string.IsNullOrEmpty(Name))
            query = query.Where(q =>
                q.Femfituser!.FirstName.Contains(Name) ||
                q.Femfituser!.LastName.Contains(Name) ||
                q.Femfituser!.UserName!.Contains(Name));

        if (!string.IsNullOrEmpty(UserId))
            query = query.Where(q => q.UserId!.Contains(UserId));

        if (!string.IsNullOrEmpty(Id))
            query = query.Where(q => q.Id == Id);

        if (IsActive.HasValue)
            query = query.Where(q => q.Femfituser!.IsActive == IsActive.Value);

        if (StartDate.HasValue)
            query = query.Where(q => q.CreatedAt >= StartDate.Value);

        if (EndDate.HasValue)
        {
            var endDate = EndDate.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            query = query.Where(q => q.CreatedAt <= endDate);
        }

        return query;
    }
}
