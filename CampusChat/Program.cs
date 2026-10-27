using CampusChat.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Shared application data.
builder.Services.AddSingleton<CampusChat.Services.CampusChatStore>();

// Student profiles and login.
builder.Services.AddSingleton<CampusChat.Services.ProfileService>();
builder.Services.AddSingleton<CampusChat.Services.UserService>();
builder.Services.AddScoped<CampusChat.Services.UserStateService>();

// Discussion threads and posts.
builder.Services.AddSingleton<CampusChat.Services.ThreadService>();
builder.Services.AddSingleton<CampusChat.Services.PostService>();
builder.Services.AddSingleton<CampusChat.Services.UnreadService>();

// Direct chats and private messages.
builder.Services.AddSingleton<CampusChat.Services.DirectChatService>();

// Keep the selected student after refreshing the browser.
builder.Services.AddScoped<
    Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage.ProtectedSessionStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();