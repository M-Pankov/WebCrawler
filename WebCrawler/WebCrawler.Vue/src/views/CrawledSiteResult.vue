<template>
   <div class="container p-3">
    <router-link :to="{ name: 'CrawledSites' }" class="btn btn-primary">Back</router-link>
    <div class="row pt-4">
        <div class="col-6">
            <h3 class="text">Results for: {{ crawledSite?.url }}</h3>
            <h4 class="text">Crawl date: {{ formatDate(crawledSite?.crawlDate!) }} </h4>
        </div>
    </div>
    <br />
    <h3>Perfomance</h3>
    <table class="table table-bordered table-striped">
        <thead>
            <tr>
                <th>
                    Url
                </th>
                <th>
                    Timings(ms)
                </th>
            </tr>
        </thead>
        <tbody>

                <tr v-for="crawlResult in crawledSite?.siteCrawlResults" v-bind:key="crawlResult.id">
                    <td>
                        {{ crawlResult?.url }}
                    </td>
                    <td>{{ crawlResult?.responseTimeMs }}</td>
                </tr>
        
        </tbody>
    </table>
    <br />
    <div v-if="crawledSite?.onlySitemapResults?.length">
        <h3>Urls not found at website :</h3>
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th>
                        Url
                    </th>
                </tr>
            </thead>
            <tbody>
                    <tr v-for="sitemapCrawlResult in crawledSite?.onlySitemapResults" v-bind:key="sitemapCrawlResult.id">
                        <td>
                            {{ sitemapCrawlResult.url }}
                        </td>
                    </tr>
            </tbody>
        </table>
    </div>
    <div v-else>
        <h4 class="text-body">Urls list founded in sitemap.xml but not founded after crawling a website is empty.</h4>
    </div>
    <br />
    <div v-if="crawledSite?.onlySiteResults?.length">
        <h3>Urls not found at sitemap.xml :</h3>
        <table class="table table-bordered table-striped">
            <thead>
                <tr>
                    <th>
                        Url
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="siteCrawlResult in crawledSite?.onlySiteResults" v-bind:key="siteCrawlResult.id">
                        <td>
                            {{ siteCrawlResult.url }}
                        </td>
                    </tr>
            </tbody>
        </table>
    </div>
    <div v-else>
        <h4 class="text-body">Urls list founded by crawling the website but not in sitemap.xml is empty.</h4>
    </div>
</div>
</template>

<script lang="ts">
            import { defineComponent, ref } from 'vue';
            import type { CrawledSite } from '@/models/CrawledSite'
            import apiService from '@/services/WebCrawlerApiService'
            export default defineComponent({
                props: ['id'],
                data() {
                    const crawledSite = ref<CrawledSite>();
                    return {
                        crawledSite
                    }
                },
                created() {
                    this.getCrawlSiteResult();
                },
                methods: {
                    async getCrawlSiteResult() {
                            
                            const response = await apiService.getCrawledSiteResults(this.id);
                            Object.assign(this, {
                            crawledSite: response.data
                            })
                    },
                    formatDate(date: string) {
                        return new Date(date).toLocaleString()
                    }
                }
            });
</script>
