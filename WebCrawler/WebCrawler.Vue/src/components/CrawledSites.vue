<template>
    
    <div class="container">
        <div class="row mt-5 justify-content-center">
            <h2 class="col-2 text-primary">Web Crawler</h2>
         </div>
         <div class="row mt-5 justify-content-center">
             <form class="col-6" @submit.prevent="crawlSite" :key="componentKey">
                <div class="input-group mb-3">
                    <input type="text" class="form-control" placeholder="Enter a website" v-model="inputUrl" aria-label="Recipient's username" aria-describedby="button-addon2">
                    <button type="submit" class="btn btn-primary" :disabled="isCrawling"  id="button-addon2"> Test </button>
                </div>
             </form>
        </div>
    <br />
    <div class="row pt-4">
        <div class="col-6">
            <h2 class="text-primary"> Test Results </h2>
        </div>
    </div>
    <br />
    <table class="table table-bordered table-striped">
        <thead>
            <tr>
                <th>
                    Url
                </th>
                <th>
                    Date
                </th>
            </tr>
        </thead>
        <tbody :key="componentKey">
            <tr v-for="site in crawledSites" :key="site.id">
                    <td>{{ site.url }}
                    </td>
                    <td>{{formatDate(site.crawlDate)}}</td>
                    <td align="center">
                        <router-link class="btn btn-primary" :class="{ disabled:isCrawling}" :to="{ name: 'LinkPerformance', params: { id: site.id } }">Show Details</router-link>
                    </td>
            </tr>
        </tbody>
    </table>
    <div class="input-group mb-3" :key="componentKey">
        <button class="btn btn-primary " :class="{ disabled: pageNumber <= 1 || isCrawling }" @click="getCrawledSites(pageNumber- 1, pageSize)"> Previous </button>
        <button class="btn btn-primary ms-1" :class="{ disabled: pageNumber >= totalPages || isCrawling}" @click="getCrawledSites(pageNumber + 1, pageSize)"> Next </button>
    </div>
    </div>
</template>

        <script lang="ts">
            import { defineComponent, ref } from 'vue';
            import type { CrawledSite } from '@/models/CrawledSite'
            import apiService from '@/services/WebCrawlerApiService'

            export default defineComponent({
                
                data() {
                    const inputUrl = ref('');
                    const crawledSites = ref<CrawledSite[]>([]);
                    const pageNumber = ref(0);
                    const pageSize = ref(0);
                    const totalPages = ref(0);
                    const isCrawling = ref(false);

                    const error = ref('');

                    return {
                        inputUrl,
                        crawledSites,
                        pageNumber,
                        pageSize,
                        totalPages,
                        isCrawling,
                        componentKey: 0,
                        error
                    }

                },
                created() {
                    this.getCrawledSites();
                },
                methods: {
                    async crawlSite() {

                        this.isCrawling = true;

                        await apiService.postCrawlSite(this.inputUrl);

                        this.getCrawledSites();
                        this.forceRerender();

                        this.isCrawling = false;
                    },
                    async getCrawledSites(page: number = 1, pageSize: number = 5) {

                            const response = await apiService.getCrawledSites(page,pageSize);
                            const data = response.data;

                            Object.assign(this, {
                            crawledSites: data.crawledSites,
                            pageNumber: data.pageNumber,
                            pageSize: data.pageSize,
                            totalPages: data.totalPages
                            })
                            
                            this.isCrawling = false;
                    },
                    forceRerender() {
                       this.componentKey += 1;
                    },
                    formatDate(date: string) {
                        return new Date(date).toLocaleString()
                    }
                }
            });
        </script>
