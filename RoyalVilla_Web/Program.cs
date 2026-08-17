using RoyalVilla_DTO;
using RoyalVilla_Web.Services;
using RoyalVilla_Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(
    o =>
    {
        o.CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        o.CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
    });

builder.Services.AddHttpClient("RoyalVillaAPI", c =>
{
    var apiUrl = builder.Configuration.GetValue<string>("ServiceUrls:VillaAPI");
    c.BaseAddress = new Uri(apiUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped<IVillaService, VillaService>();
builder.Services.AddScoped<IBaseServices, BaseService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
