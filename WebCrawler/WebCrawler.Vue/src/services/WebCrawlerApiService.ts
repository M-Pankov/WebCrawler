import axios from 'axios'

const client = axios.create()
const apiUrl = process.env.VUE_APP_APIURL;

export default {
    async execute(method: string, resource: string) {
        return client({
            method,
            url:apiUrl + resource
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