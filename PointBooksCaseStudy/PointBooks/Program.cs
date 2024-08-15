using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PointBooks.Data;
using PointBooks.Models;
using PointBooks.Repositories;
using PointBooks.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//builder.Services.AddScoped<IGenericRepository<Book>, BookRepository>();
builder.Services.AddScoped<IGenericRepository<Author>, AuthorRepository>();

builder.Services.AddScoped<IGenericRepository<CartItem>, CartItemRepository>();
builder.Services.AddScoped<IGenericRepository<Cart>, CartRepository>();
builder.Services.AddScoped<IGenericRepository<Category>,CategoryRepository >();
builder.Services.AddScoped<IGenericRepository<OrderItem>, OrderItemRepository>();
builder.Services.AddScoped<IGenericRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IGenericRepository<Publisher>, PublisherRepository>();
builder.Services.AddScoped<IGenericRepository<Role>, RoleRepository>();
builder.Services.AddScoped<IGenericRepository<User>, UserRepository >();

builder.Services.AddScoped<BookAuthorRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<PublisherRepository>();

builder.Services.AddScoped<GenericRepository<Book>>(provider =>
{
    var context = provider.GetRequiredService<DapperContext>();
    return new GenericRepository<Book>(context, "Books", "BookID", new Dictionary<string, string>());
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


builder.Services.AddHttpClient();
var app = builder.Build();

app.UseCors();

//Initialize the database
app.InitializeDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllers();

app.Run();
