using GSVNexus.API.Data;
using GSVNexus.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<GSVNexusDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Frontend");
app.UseHttpsRedirection();

app.MapGet("/api/health", async (GSVNexusDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();

    return canConnect
        ? Results.Ok(new HealthStatus(
            "Healthy",
            "GSVNexus",
            DateTime.UtcNow))
        : Results.Json(
            new HealthStatus("Unavailable", "GSVNexus", DateTime.UtcNow),
            statusCode: StatusCodes.Status503ServiceUnavailable);
});

app.MapGet("/api/dms/cases", async (GSVNexusDbContext db) =>
{
    var cases = await db.DmsCases
        .AsNoTracking()
        .OrderByDescending(item => item.CreatedAt)
        .Select(item => new
        {
            item.Id,
            item.CaseNumber,
            item.Title,
            item.DocumentTypeCode,
            item.ProjectCode,
            item.Status,
            item.CurrentStep,
            item.AssignedRole,
            item.AssignedTo,
            item.DueAt,
            item.CreatedAt,
        })
        .ToListAsync();

    return Results.Ok(cases);
});

app.MapPost("/api/dms/cases", async (
    DmsCreateCaseRequest request,
    GSVNexusDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(request.Title)
        || string.IsNullOrWhiteSpace(request.DocumentTypeCode))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["title"] = string.IsNullOrWhiteSpace(request.Title)
                ? ["Naziv predmeta je obvezan."]
                : [],
            ["documentTypeCode"] = string.IsNullOrWhiteSpace(request.DocumentTypeCode)
                ? ["Vrsta dokumenta je obvezna."]
                : [],
        });
    }

    var caseNumber = await GenerateCaseNumberAsync(db, request.ProjectCode, request.DocumentTypeCode);
    var caseItem = new DmsCase
    {
        CaseNumber = caseNumber,
        Title = request.Title.Trim(),
        DocumentTypeCode = request.DocumentTypeCode.Trim().ToUpperInvariant(),
        ProjectCode = request.ProjectCode?.Trim().ToUpperInvariant(),
        Status = "Zaprimljeno",
        CurrentStep = "Zaprimanje",
        AssignedRole = request.AssignedRole?.Trim(),
        AssignedTo = request.AssignedTo?.Trim(),
        DueAt = request.DueAt,
    };

    db.DmsCases.Add(caseItem);
    db.DmsAuditEvents.Add(new DmsAuditEvent
    {
        EntityType = "DmsCase",
        EntityId = caseItem.Id,
        Action = "Created",
        Actor = request.Actor ?? "System",
        Comment = "Predmet je stvoren iz ulaznog DMS redoslijeda.",
        PayloadJson = $$"""{"caseNumber":"{{caseNumber}}","status":"Zaprimljeno"}""",
    });

    await db.SaveChangesAsync();
    return Results.Created($"/api/dms/cases/{caseItem.Id}", caseItem);
});

app.MapPost("/api/dms/cases/{id:guid}/transition", async (
    Guid id,
    DmsTransitionRequest request,
    GSVNexusDbContext db) =>
{
    var caseItem = await db.DmsCases.FindAsync(id);
    if (caseItem is null)
    {
        return Results.NotFound();
    }

    var allowedTransitions = new Dictionary<string, string[]>
    {
        ["Zaprimljeno"] = ["Evidentirano"],
        ["Evidentirano"] = ["Dodijeljeno"],
        ["Dodijeljeno"] = ["U obradi", "Vraćeno"],
        ["U obradi"] = ["Na odobravanju", "Vraćeno"],
        ["Na odobravanju"] = ["Odobreno", "Odobreno s primjedbama", "Odbijeno", "Vraćeno"],
        ["Odobreno"] = ["Distribuirano"],
        ["Odobreno s primjedbama"] = ["Distribuirano", "Vraćeno"],
        ["Vraćeno"] = ["U obradi", "Evidentirano"],
        ["Odbijeno"] = ["Arhivirano"],
        ["Distribuirano"] = ["Arhivirano"],
    };

    if (!allowedTransitions.TryGetValue(caseItem.Status, out var allowed)
        || !allowed.Contains(request.Status, StringComparer.Ordinal))
    {
        return Results.ValidationProblem(
            new Dictionary<string, string[]>
            {
                ["status"] = [$"Prijelaz '{caseItem.Status}' → '{request.Status}' nije dopušten."]
            });
    }

    var previousStatus = caseItem.Status;
    caseItem.Status = request.Status;
    caseItem.CurrentStep = request.CurrentStep ?? request.Status;
    caseItem.AssignedTo = request.AssignedTo ?? caseItem.AssignedTo;
    caseItem.UpdatedAt = DateTime.UtcNow;

    db.DmsAuditEvents.Add(new DmsAuditEvent
    {
        EntityType = "DmsCase",
        EntityId = caseItem.Id,
        Action = "StatusTransition",
        Actor = request.Actor,
        Comment = request.Comment,
        PayloadJson = $$"""{"from":"{{previousStatus}}","to":"{{request.Status}}"}""",
    });

    await db.SaveChangesAsync();
    return Results.Ok(caseItem);
});

app.Run();

static async Task<string> GenerateCaseNumberAsync(
    GSVNexusDbContext db,
    string? projectCode,
    string documentTypeCode)
{
    var prefix = string.IsNullOrWhiteSpace(projectCode)
        ? "DMS"
        : projectCode.Trim().ToUpperInvariant();
    var typeCode = documentTypeCode.Trim().ToUpperInvariant();
    var year = DateTime.UtcNow.Year;
    var count = await db.DmsCases.CountAsync(
        item => item.ProjectCode == prefix
            && item.DocumentTypeCode == typeCode
            && item.CreatedAt.Year == year) + 1;

    return $"{prefix}-{typeCode}-{count:0000}";
}

public sealed record DmsTransitionRequest(
    string Status,
    string Actor,
    string? CurrentStep = null,
    string? AssignedTo = null,
    string? Comment = null);

public sealed record DmsCreateCaseRequest(
    string Title,
    string DocumentTypeCode,
    string? ProjectCode = null,
    string? AssignedRole = null,
    string? AssignedTo = null,
    DateTime? DueAt = null,
    string? Actor = null);
