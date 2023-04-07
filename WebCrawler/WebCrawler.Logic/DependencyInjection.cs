﻿using Microsoft.Extensions.DependencyInjection;
using WebCrawler.Logic.Crawlers;
using WebCrawler.Logic.Parsers;
using WebCrawler.Logic.Services;
using WebCrawler.Logic.Validators;

namespace WebCrawler.Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddLogicServices(this IServiceCollection services)
    {
        services.AddScoped<UrlValidator>();
        services.AddHttpClient<HtmlLoaderService>();
        services.AddScoped<HtmlLoaderService>();
        services.AddScoped<SitemapLoaderService>();
        services.AddScoped<HtmlParser>();
        services.AddScoped<SitemapCrawler>();
        services.AddScoped<SiteCrawler>();
        services.AddScoped<Crawler>();

        return services;
    }
}