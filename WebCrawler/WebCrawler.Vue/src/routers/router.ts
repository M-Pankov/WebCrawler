import {createRouter,createWebHistory} from 'vue-router'
import CrawledSites from '@/views/CrawledSites.vue'
import CrawledSiteResults from '@/views/CrawledSiteResult.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            name: 'CrawledSites',
            component: CrawledSites
        },
        {
          path: '/siteResults/:id',
          name: 'LinkPerformance',
          component: CrawledSiteResults,
          props: true
        },
        {
          path: '/:pathMatch(.*)*',
          redirect: '/'
        }
    ]
})
export default router