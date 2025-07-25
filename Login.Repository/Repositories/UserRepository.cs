﻿using Login.Domain.Entities;
using Login.Domain.Interfaces.Repositories;
using Login.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace Login.Repository.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PostgresContext _context;

    public UserRepository(PostgresContext context)
    {
        _context = context;
    }

    public async Task<List<ApplicationUser>> ListUsers()
    {
        List<ApplicationUser> list = await _context.User.ToListAsync();

        return list;
    }

    public async Task<ApplicationUser> GetUser(string userId)
    {
        ApplicationUser user = await _context.User.FindAsync(userId);

        return user;
    }

    public async Task<ApplicationUser> CreateUser(ApplicationUser user)
    {
        var ret = await _context.User.AddAsync(user);

        await _context.SaveChangesAsync();

        ret.State = EntityState.Detached;

        return ret.Entity;
    }

    public async Task<int> UpdateUser(ApplicationUser user)
    {
        _context.Entry(user).State = EntityState.Modified;

        return await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteUser(string userId)
    {
        var item = await _context.User.FindAsync(userId);
        _context.User.Remove(item);

        await _context.SaveChangesAsync();

        return true;
    }
}
