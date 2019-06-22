<template>
  <div id="app">
    <main-navbar />

    <main role="main" class="container mt-4">
      <router-view/>
    </main>

    <login-modal />
  </div>
</template>

<script>
import { mapActions } from 'vuex'
import MainNavbar from './components/main-navbar'
import LoginModal from './components/login-modal'

export default {
  name: 'App',
  components: {
    MainNavbar,
    LoginModal
  },
  mounted () {
    this.loadProfile()
  },
  methods: {
    ...mapActions('context', [
      'loadProfile'
    ])
  }
}
</script>
