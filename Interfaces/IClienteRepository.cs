using System.Linq.Expressions;
using DesafioSidequestMinimalAPI.Models;

namespace DesafioSidequestMinimalAPI.Interfaces;

public interface IClienteRepository : IDisposable 
{
    Task<int> Adicionar(Cliente cliente);
    Task<int> Atualizar(Cliente cliente);
    Task<int> Remover(Guid id);
    Task<List<Cliente>> ObterTodos();
    Task<Cliente?> ObterPorId(Guid id);
    Task<IEnumerable<Cliente>> Buscar(Expression<Func<Cliente, bool>> predicate);
}