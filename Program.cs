using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. تفعيل صفحات Razor
builder.Services.AddRazorPages();

// ✅ 2. تفعيل الجلسات
builder.Services.AddDistributedMemoryCache(); // مهم جدًا للجلسة
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // مدة الجلسة 30 دقيقة
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// ✅ 3. إعداد الـ Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// ✅ 4. تفعيل صفحات Razor
app.MapRazorPages();

app.UseRouting();
app.UseSession();

app.MapRazorPages();

// عند الوصول إلى "/" نتحقق أولاً من الجلسة ثم نعيد التوجيه
app.MapGet("/", async context =>
{
    var user = context.Session.GetString("User");
    if (string.IsNullOrEmpty(user))
        context.Response.Redirect("/Login");

    await Task.CompletedTask;
});


// ✅ 6. تشغيل التطبيق
app.Run();
