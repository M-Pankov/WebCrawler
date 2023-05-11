
export class CrawledSiteResult {

    public id : number;
    public url: string;
    public responseTimeMs: number;

    constructor(Id:number, Url: string, ResponseTimeMs: number) {
        this.id = Id,
        this.url = Url;
        this.responseTimeMs = ResponseTimeMs;

    }
}