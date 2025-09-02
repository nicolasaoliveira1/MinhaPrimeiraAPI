var builder = WebApplication.CreateBuilder(args);

List<Tarefa> tarefas = new List<Tarefa>();
int nextId = 1;


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//GET ALL (retorna todas tarefas)
app.MapGet("/tarefas", () =>
{
    return Results.Ok(tarefas);
});

//GET ID (retorna tarefa pelo ID)
app.MapGet("/tarefas/{id:int}", (int id) =>
{
    Tarefa tarefaEncontrada = null;
    for (int i = 0; i < tarefas.Count; i++)
    {
        if (tarefas[i].Id == id)
        {
            tarefaEncontrada = tarefas[i];
            break;
        }
    }
    if (tarefaEncontrada != null)
    {
        return Results.Ok(tarefaEncontrada);
    }
    else
    {
        return Results.NotFound();
    }
});
//DELETE (exclui tarefa)
app.MapDelete("/tarefas/{id:int}", (int id) =>
{
    var encontrada = tarefas.FirstOrDefault(p => p.Id == id);
    if (encontrada == null)
    {
        return Results.NotFound();
    }
    else
    {
        tarefas.Remove(encontrada);
        return Results.NoContent();
    }
});

//POST CREATE (cria uma nova tarefa)
app.MapPost("/tarefas", (Tarefa entrada) =>
{
    if (entrada.Titulo == null)
    {
        return Results.BadRequest(new { erro = "Bote o título, porra!" });
    }
    Tarefa novaTarefa = new Tarefa();
    novaTarefa.Id = nextId++;
    novaTarefa.Titulo = entrada.Titulo;
    novaTarefa.Concluido = entrada.Concluido;
    tarefas.Add(novaTarefa);
    return Results.Created($"/tarefas/{novaTarefa.Id}", novaTarefa);
});

//PUT (editar uma tarefa)




app.Run();
