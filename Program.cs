var builder = WebApplication.CreateBuilder(args);

// ✅ Add services to the container before calling `builder.Build()`
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

// ✅ Add authentication (before builder.Build)
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; 
        options.AccessDeniedPath = "/Account/AccessDenied"; 
        options.ExpireTimeSpan = TimeSpan.FromDays(7);  // 🔹 Keeps user logged in for 7 days
        options.SlidingExpiration = true;  // 🔹 Extends session if user is active
    });

builder.Services.AddAuthorization();

var app = builder.Build(); // 🔹 No modifications to `builder.Services` after this point

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔹 Ensures cookie policies are properly handled
app.UseCookiePolicy();

// ✅ Authentication should be before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();