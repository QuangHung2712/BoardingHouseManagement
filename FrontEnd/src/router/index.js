import { createRouter, createWebHistory } from 'vue-router'
import ManageRouter from './ManageRouter'


const routes = [
  ...ManageRouter,
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
  {
    path: '/',
    name: 'home',
    component:() => import('../views/HomeView.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
