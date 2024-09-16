import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    name: 'home',
    component: () => import('../views/HomeView.vue')
  },
  {
    path: '/manage',
    name: 'manage',
    component: () => import('../views/Manage/HomeManage.vue'),
    children: [
      {
        path: '',
        name: 'towerpage',
        component: () => import('../views/Manage/Layout/TowerView.vue'),
      },
      {
        path: ':idtower',
        name: 'towerDetails',
        component: () => import('../views/Manage/Layout/TowerDetail.vue'),
        children:[
          {
            path: 'homepage',
            name: 'homepage',
            component: () => import('../views/Manage/Layout/HomePageView.vue')
          },
          {
            path: 'room',
            name: 'roompage',
            component: () => import('../views/Manage/Layout/RoomView.vue')
          },
          {
            path: 'service',
            name: 'servicepage',
            component: () => import('../views/Manage/Layout/ServiceView.vue')
          },
        ]
      },
    ]
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
