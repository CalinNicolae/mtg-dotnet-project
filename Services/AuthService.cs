
using MTGProject.Models;
using String = System.String;

namespace MTGProject.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
public class AuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _nav;
    private readonly string _supabaseUrl = "https://szraljeitrioekgucyvz.supabase.co";
    private readonly string _supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InN6cmFsamVpdHJpb2VrZ3VjeXZ6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3Mzk5NjkwODYsImV4cCI6MjA1NTU0NTA4Nn0.OSLZf4AeLRXodJFNtR5UF_H5ThvNM5r4aNfqU8Dhcok";
    private string _token;
    public string Id{ get; set; }
    private string Email{ get; set; }
    private const string STORAGE_KEY_PREFIX = "mtg_filters_";
    private string Token 
    { 
        get => _token; 
         set
        {
            _token = value;
            AuthenticationStateChanged?.Invoke(this, EventArgs.Empty); // Notify subscribers
        }
    }
    
    public event EventHandler AuthenticationStateChanged;
    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
    
    private readonly Supabase.Client _supabase;
    
    public AuthService(HttpClient http, ILocalStorageService localStorage, NavigationManager nav)
    {
        _http = http;
        _localStorage = localStorage;
        _nav = nav;
        _http.DefaultRequestHeaders.Add("apikey", _supabaseKey);
    }

    // 🔹 Load token from LocalStorage when app starts
    public async Task InitializeAuth()
    {
        Token = await _localStorage.GetItemAsync<string>("auth_token");
        Email = await _localStorage.GetItemAsync<string>("email");
        Id = await _localStorage.GetItemAsync<string>("uid");
    }

    // 🔹 Login and store token
    public async Task<bool> LoginAsync(string email, string password)
    {
        HttpResponseMessage  response = await _http.PostAsJsonAsync($"{_supabaseUrl}/auth/v1/token?grant_type=password", new { email, password });
        string content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            TokenResponse tokenResponse = TokenResponse.FromJson(content);
            Token = tokenResponse.AccessToken;
            Id = tokenResponse.User.Id.ToString();
            await SaveUserLocally(tokenResponse);
            return true;
        }
        return false;
    }


    public async Task SaveUserLocally(TokenResponse tokenResponse)
    {
        await _localStorage.SetItemAsync("auth_token", Token);
        await _localStorage.SetItemAsync("email", tokenResponse.User.Email);
        await _localStorage.SetItemAsync("uid", tokenResponse.User.Id);
    }

    // 🔹 Logout and remove token from LocalStorage
    public async Task LogoutAsync()
    {
        Token = null;
        await _localStorage.RemoveItemAsync("auth_token");
        await _localStorage.RemoveItemAsync("email");
        await _localStorage.RemoveItemAsync("uid");
        await _localStorage.RemoveItemAsync($"{STORAGE_KEY_PREFIX}searchTerm");
        await _localStorage.RemoveItemAsync($"{STORAGE_KEY_PREFIX}manaCostFilter");
        await _localStorage.RemoveItemAsync($"{STORAGE_KEY_PREFIX}typeFilter");
        await _localStorage.RemoveItemAsync($"{STORAGE_KEY_PREFIX}rarityFilter");
        await _localStorage.RemoveItemAsync($"{STORAGE_KEY_PREFIX}colorFilter");
        _nav.NavigateTo("/login", forceLoad: true);
    }

    public async Task<bool> RegisterAsync(string email, string password)
    {
        HttpResponseMessage response = await _http.PostAsJsonAsync($"{_supabaseUrl}/auth/v1/signup", new { email, password });
        if (response.IsSuccessStatusCode)
        {
            String content = await response.Content.ReadAsStringAsync();
            TokenResponse tokenResponse = TokenResponse.FromJson(content);
            Token = tokenResponse.AccessToken;
            Id = tokenResponse.User.Id.ToString();
            await SaveUserLocally(tokenResponse);
            return true;
        }

        return false; 
    }

}
