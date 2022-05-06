using DesafioSidequestMinimalAPI.Data;
using DesafioSidequestMinimalAPI.Interfaces;
using DesafioSidequestMinimalAPI.Models;
using DesafioSidequestMinimalAPI.Repository;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MeuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/cliente/obter-todos", async (IClienteRepository clienteRepository) =>
     await clienteRepository.ObterTodos())
    .WithName("GetCliente")
    .WithTags("Cliente");

app.MapGet("/cliente/obter-por-id/{id}", async (
            Guid id,
            IClienteRepository clienteRepository) =>
        await clienteRepository.ObterPorId(id)
            is { } cliente
            ? Results.Ok(cliente)
            : Results.NotFound())
    .Produces<Cliente>()
    .Produces(StatusCodes.Status404NotFound)
    .WithName("ObterClientePorId")
    .WithTags("Cliente");

app.MapPost("/cliente/adicionar", async (IClienteRepository clienteRepository, Cliente cliente) =>
    {
        if (!MiniValidator.TryValidate(cliente, out var errors))
            return Results.ValidationProblem(errors);

        var clienteBanco = clienteRepository.Buscar(c => c.Documento == cliente.Documento).Result.Any();
        if (clienteBanco)
            return Results.Conflict("Cliente já cadastrado");

        var result = await clienteRepository.Adicionar(cliente);

        return result > 0
            ? Results.CreatedAtRoute("ObterClientePorId", new { id = cliente.Id }, cliente)
            : Results.BadRequest("Houve um problema ao salvar o registro");
    })
    .ProducesValidationProblem()
    .Produces<Cliente>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status409Conflict)
    .WithName("AdicionarCliente")
    .WithTags("Cliente");

app.MapPut("/cliente/atualizar/{id}", async (Guid id,IClienteRepository clienteRepository, Cliente cliente) =>
    {
        var clienteBanco = clienteRepository.ObterPorId(id).Result;
        if (clienteBanco is null)
            return Results.BadRequest();

        if (!MiniValidator.TryValidate(cliente, out var errors))
            return Results.ValidationProblem(errors);

        var result = await clienteRepository.Atualizar(cliente);

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Houve um problema ao atualizar o registro");
    })
    .ProducesValidationProblem()
    .Produces<Cliente>(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("AtualizarCliente")
    .WithTags("Cliente");

app.MapDelete("/cliente/excluir/{id}", async (Guid id, IClienteRepository clienteRepository) =>
    {
        var clienteBanco = clienteRepository.ObterPorId(id).Result;
        if (clienteBanco is null)
            return Results.BadRequest();
        
        var result = await clienteRepository.Remover(id);

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Houve um problema ao excluir o registro");
    })
    .ProducesValidationProblem()
    .Produces<Cliente>(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("ExcluirCliente")
    .WithTags("Cliente");

app.Run();

