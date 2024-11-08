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
            path: 'home',
            name: 'home',
            component: () => import('../views/Manage/Layout/HomePageView.vue')
          },
          {
            path: 'room',
            name: 'room',
            component: () => import('../views/Manage/Layout/RoomView.vue')
          },
          {
            path: 'service',
            name: 'service',
            component: () => import('../views/Manage/Layout/ServiceView.vue')
          },
          {
            path: 'contract',
            name: 'contract',
            component: () => import('../views/Manage/Layout/Contract/ContractView.vue'),
          },
          {
            path: 'createEdit/:idcontract',
            name: 'createEdit',
            component:()=> import('../views/Manage/Layout/Contract/CreateEditContract.vue')
          },
          {
            path: 'arise',
            name: 'arise',
            component:()=> import('../views/Manage/Layout/AriseView.vue')
          },
          {
            path: 'bill',
            name: 'bill',
            component:()=> import('../views/Manage/Layout/BillView.vue')
          }
        ]
      },
    ]
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/Manage/Login/PageLogin.vue')
  },
  {
    path: '/forgot',
    name: 'forgot',
    component: () => import('../views/Manage/Login/ForgotPassword.vue')
  },
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
