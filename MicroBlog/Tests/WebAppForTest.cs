using MicroBlog.WebAppConfigs;
using Microsoft.AspNetCore.Builder;

namespace Tests;

public static class WebAppForTest
{
    private static WebApplication? _app;
    
    public static WebApplication GetTestApp()
    {
        if (_app != null) return _app;
        
        var builder = WebApplication.CreateBuilder();

        BuilderSetUp.SetUp(builder);
        
        _app = builder.Build();
        
        return _app;
    }
}