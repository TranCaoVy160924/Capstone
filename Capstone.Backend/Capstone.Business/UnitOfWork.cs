using Capstone.Database.EF;
using Capstone.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Business;
public class UnitOfWork
{
    private readonly ApplicationDbContext _context;
    public RepositoryBase<AppUser> UserRepo { get; internal set; }

    public UnitOfWork()
    {
        _context = new ApplicationDbContext();
        UserRepo = new RepositoryBase<AppUser>(_context);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
