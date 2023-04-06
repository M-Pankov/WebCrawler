using System;

namespace WebCrawler.Persistence.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public Uri Url { get; set; }
}
