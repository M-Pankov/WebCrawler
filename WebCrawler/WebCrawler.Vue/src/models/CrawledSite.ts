import { CrawledSiteResult } from "./CrawledSiteResult";

export class CrawledSite {
    public id: number;
    public url: string;
    public crawlDate: string;
    public siteCrawlResults: CrawledSiteResult[] | null;
    public onlySitemapResults: CrawledSiteResult[] | null;
    public onlySiteResults: CrawledSiteResult[] | null;

    constructor(Id: number, Url: string, CrawlDate: string, SiteCrawlResults: CrawledSiteResult[],
        OnlySitemapResults: CrawledSiteResult[], OnlySiteResults: CrawledSiteResult[]) {

        this.id = Id;
        this.url = Url;
        this.crawlDate = CrawlDate;
        this.siteCrawlResults = SiteCrawlResults;
        this.onlySitemapResults = OnlySitemapResults;
        this.onlySiteResults = OnlySiteResults;
    }
}
