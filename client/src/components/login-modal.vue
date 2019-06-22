<template>
  <b-modal id="loginModal" ref="loginModal" hide-footer title="Login" @hidden="onHidden">
    <b-form @submit.prevent="onSubmit" @reset.prevent="onCancel">
      <b-alert show variant="warning">In this test app, any credentials are valid!</b-alert>
      <b-form-group label="Email:" label-for="emailInput">
        <b-form-input id="emailInput"
                      type="email"
                      v-model="form.email"
                      required
                      placeholder="Enter your email address">
        </b-form-input>
      </b-form-group>
      <b-form-group label="Password:" label-for="passwordInput">
        <b-form-input id="passwordInput"
                      type="password"
                      v-model="form.password"
                      required
                      placeholder="Enter your password">
        </b-form-input>
      </b-form-group>
      <b-form-group label="Authentication mode">
        <b-form-radio-group
          id="authMode"
          v-model="authMode"
          :options="authOptions"/>
      </b-form-group>

      <button class="btn btn-primary float-right ml-2" type="submit">Login</button>
      <button class="btn btn-secondary float-right" type="reset">Cancel</button>
    </b-form>
  </b-modal>
</template>

<script>
import { mapActions } from 'vuex'

export default {
  data () {
    return {
      form: {
        email: '',
        password: ''
      },
      authMode: 'cookie',
      authOptions: [
        { text: 'Cookie', value: 'cookie' },
        { text: 'JWT Bearer', value: 'jwt' }
      ]
    }
  },
  methods: {
    ...mapActions('context', [
      'login'
    ]),
    onSubmit (evt) {
      this.login({ authMethod: this.authMode, credentials: this.form }).then(() => {
        this.$refs.loginModal.hide()
      })
    },
    onCancel (evt) {
      this.$refs.loginModal.hide()
    },
    onHidden () {
      Object.assign(this.form, {
        email: '',
        password: ''
      })
    }
  }
}
</script>
