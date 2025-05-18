using BookIt.Domain.Entities;
using BookIt.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace BookIt.Persistence.Implementations.Repositories.Generic;

public abstract class IdentityRepository<TUser> where TUser : IdentityUser
{
    protected readonly AppDbContext _dbContext;
    protected readonly UserManager<TUser> _userManager;
    protected readonly RoleManager<IdentityRole> _roleManager;

    protected IdentityRepository(
        AppDbContext dbContext,
        UserManager<TUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public virtual async Task<TUser> CreateAsync(TUser entity)
    {
        var result = await _userManager.CreateAsync(entity);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        return entity;
    }

    public virtual TUser Update(TUser entity)
    {
        var result = _userManager.UpdateAsync(entity).Result;
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        return entity;
    }

    public virtual void HardDelete(TUser entity)
    {
        var result = _userManager.DeleteAsync(entity).Result;
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public virtual void SoftDelete(TUser entity)
    {
        if (entity is ApplicationUser user)
        {
            user.IsDeleted = true;
            Update(entity);
        }
    }

    public virtual void Repair(TUser entity)
    {
        if (entity is ApplicationUser user)
        {
            user.IsDeleted = false;
            Update(entity);
        }
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public virtual IQueryable<TUser> GetAll(
        Expression<Func<TUser, bool>>? predicate = null,
        Func<IQueryable<TUser>, IIncludableQueryable<TUser, object>>? include = null,
        bool ignoreFilter = false)
    {
        var query = _userManager.Users.AsQueryable();

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (include != null)
        {
            query = include(query);
        }

        if (ignoreFilter)
        {
            query = query.IgnoreQueryFilters();
        }

        return query;
    }

    public virtual async Task<TUser?> GetAsync(string id, bool ignoreFilter = false)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public virtual async Task<TUser?> GetAsync(
        Expression<Func<TUser, bool>> predicate,
        Func<IQueryable<TUser>, IIncludableQueryable<TUser, object>>? include = null,
        bool ignoreFilter = false)
    {
        var query = _userManager.Users.AsQueryable();

        if (include != null)
        {
            query = include(query);
        }

        if (ignoreFilter)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.FirstOrDefaultAsync(predicate);
    }

    public virtual IQueryable<TUser> Paginate(IQueryable<TUser> query, int limit, int page = 1)
    {
        return query.Skip((page - 1) * limit).Take(limit);
    }

    public virtual IQueryable<TUser> OrderBy(IQueryable<TUser> query, Expression<Func<TUser, object>> predicate)
    {
        return query.OrderBy(predicate);
    }

    public virtual IQueryable<TUser> OrderByDescending(IQueryable<TUser> query, Expression<Func<TUser, object>> predicate)
    {
        return query.OrderByDescending(predicate);
    }

    public virtual async Task<bool> IsExistAsync(
        Expression<Func<TUser, bool>> predicate,
        Func<IQueryable<TUser>, IIncludableQueryable<TUser, object>>? include = null,
        bool ignoreFilter = false)
    {
        var query = _userManager.Users.AsQueryable();

        if (include != null)
        {
            query = include(query);
        }

        if (ignoreFilter)
        {
            query = query.IgnoreQueryFilters();
        }

        return await query.AnyAsync(predicate);
    }

    public virtual async Task CreateRangeAsync(IEnumerable<TUser> entities)
    {
        foreach (var entity in entities)
        {
            await CreateAsync(entity);
        }
    }
} 