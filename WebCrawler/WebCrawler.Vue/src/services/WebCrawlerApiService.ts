import axios from 'axios'

const baseURL = 'https://localhost:7061/api/WebCrawler';

const client = axios.create()

export default {
    async execute(method: string, resource: string) {
        return client({
            method,
            url:baseURL + resource
        }).then(req => {
            return req
        })
    },

    getCrawledSites(page: number , size: number) {
        
        const paging = '?pageNumber=' + page + '&' + 'pageSize=' + size;

        return this.execute('get','/crawled-sites' + paging)
    },

    getCrawledSiteResults(id: number) {
        return this.execute('get', '/crawled-sites/'+ id +'/results')
    },

    postCrawlSite(uriString: string) {
        return this.execute('post', `/crawl-site?uriString=` + uriString)
    }
}