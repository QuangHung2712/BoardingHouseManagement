import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'HomePage',
    component: () => import('../views/Manage/HomeView.vue'),
    children: [
      {
        path: 'homepage',
        name: 'homepage',
        component: () => import('../views/Manage/Layout/HomePageView.vue')
      },
      {
        path: 'roompage',
        name: 'roompage',
        component: () => import('../views/Manage/Layout/RoomView.vue')
      },
    ]
  },
  {
    path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
