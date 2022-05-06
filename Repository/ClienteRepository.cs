using System.Linq.Expressions;
using DesafioSidequestMinimalAPI.Data;
using DesafioSidequestMinimalAPI.Interfaces;
using DesafioSidequestMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioSidequestMinimalAPI.Repository;

public class ClienteRepository : IClienteRepository
{
    private readonly MeuDbContext _context;

    public ClienteRepository(MeuDbContext context)
    {
        _context = context;
    }
    public async Task<int> Adicionar(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        return await SaveChanges();
    }

    public async Task<int> Atualizar(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        return await SaveChanges();
    }

    public  async Task<List<Cliente>> ObterTodos()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> ObterPorId(Guid id)
    {
        return await _context.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Cliente>> Buscar(Expression<Func<Cliente, bool>> predicate)
    {
        return await _context.Clientes.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<int> Remover(Guid id)
    {
        _context.Remove(new Cliente { Id = id });
        return await SaveChanges();
    }

    private async Task<int> SaveChanges()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}