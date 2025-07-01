var builder = WebApplication.CreateBuilder(args);

//  加入 CORS 服務
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost63343", policy =>
    {
        policy.WithOrigins("http://localhost:63343")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 使用剛剛加的 CORS 設定（放在 UseRouting 和 UseAuthorization 之間）
app.UseCors("AllowLocalhost63343");

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
