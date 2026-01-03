using ReThread.Application.Interfaces;
using ReThreaded.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ReThread.Infrastructure.Persistence
{

        public class UnitOfWork : IUnitOfWork
    {
            private readonly ApplicationDbContext _context;

            public UnitOfWork(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task SaveChangesAsync()
            {
                await _context.SaveChangesAsync();
            }
        }
    

}
