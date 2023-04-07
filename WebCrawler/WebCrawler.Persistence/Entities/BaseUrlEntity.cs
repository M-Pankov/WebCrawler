using System;

namespace WebCrawler.Persistence.Entities;

public class BaseUrlEntity
{
    public int Id { get; set; }
    public Uri Url { get; set; }
}
