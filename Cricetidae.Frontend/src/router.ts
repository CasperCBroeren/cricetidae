import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import TopDelta from './views/TopDelta.vue';
import About from './views/About.vue';

Vue.use(Router);

export default new Router({
    routes: [
        {
            path: '/',
            name: 'home',
            component: Home,
        },
        {
            path: '/TopDelta/:store',
            name: 'topDelta',
            component: TopDelta,
            props: (route) => ({
                store: route.params.store,
            }),
        },
        {
            path: '/about',
            name: 'about',
            component: About,
        },
    ],
});
