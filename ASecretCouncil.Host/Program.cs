using ASecretCouncil.Model.Application;
using ASecretCouncil.Model.Resume;
using MudBlazor.Services;
using ASecretCouncil.Host.Components;
using ASecretCouncil.Host.Components.Pages;
using ASecretCouncil.Host.Components.Pages.Acknowledgement;
using ASecretCouncil.Host.Components.Pages.Files;
using ASecretCouncil.Host.Components.Pages.PersonalInformation;
using ASecretCouncil.Host.Components.Pages.Steps;
using ASecretCouncil.Host.Components.Pages.Summary;
using ASecretCouncil.Host.Shared.Mapping;
using ASecretCouncil.Host.Shared.Services;
using ASecretCouncil.Model.Data;
using ASecretCouncil.Model.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents(options =>
        options.DetailedErrors = builder.Environment.IsDevelopment())
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<LoadingService>();

builder.Services.AddAutoMapper([typeof(AppMappingProfile), typeof(ModelMappingProfile)]);

builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IApplicationStepService, ApplicationStepService>();
builder.Services.AddScoped<ReturningApplicationProvider>();
builder.Services.AddScoped<IFormSteps, StaticFormSteps>();
builder.Services.AddScoped<FormStepRoutingPresenter>();
builder.Services.AddScoped<IPersonalInformationPresenter, DefaultPersonalInformationPresenter>();
builder.Services.AddScoped<IFilesPresenter, DefaultFilesPresenter>();
builder.Services.AddScoped<IFormRoutingPresenter, DefaultFormRoutingPresenter>();
builder.Services.AddScoped<PrefillFormAfterRenderDelegate>();

builder.Services.AddScoped<IAcknowledgmentPresenter, DefaultAcknowledgementPresenter>();
builder.Services.AddScoped<ISummaryPresenter, DefaultSummaryPresenter>();

builder.Services.AddDbContext<ApplicationContext>();

builder.Services.AddMudServices();

#if DEBUG
builder.Services.AddSassCompiler();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    using var scope = app.Services.CreateAsyncScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    context.Database.EnsureCreated();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
