import { createApp } from 'vue'
import App from './App.vue'
import Router from './routers/router'
import 'bootstrap/dist/css/bootstrap.css'

const app = createApp(App)
app.use(Router)

app.mount('#app')