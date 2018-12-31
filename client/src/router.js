import Vue from 'vue'
import Router from 'vue-router'
import HomePage from '@/views/home'
import QuestionPage from '@/views/question'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Home',
      component: HomePage
    },
    {
      path: '/question/:id',
      name: 'Question',
      component: QuestionPage
    }
  ]
})
