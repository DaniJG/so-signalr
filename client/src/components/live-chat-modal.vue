<template>
  <b-modal id="liveChatModal" ref="liveChatModal" hide-footer title="Live Chat" size="lg" @hidden="onHidden">

    <div class="bg-light messages-container">
      <ul v-if="messages.length" class="list-unstyled container">
        <li v-for="(message, index) in messages" :key="index" class="row my-2">
          <span class="col-3">{{ message.username === profile.name ? 'You' : message.username }}</span>
          <vue-markdown :class="{'col-9': true, 'text-muted': message.username === profile.name}" :source="message.text" />
        </li>
      </ul>
      <p v-else class="text-muted text-center">
        Welcome to the chat...<br />
        Say hi!
      </p>
    </div>

    <b-form class="border-top mt-2 pt-2" @submit.prevent="onSendMessage">
      <b-form-group label="Your message:" label-for="messageInput">
        <b-form-textarea id="messageInput"
                      v-model="form.message"
                      placeholder="What do you have to say?"
                      :rows="2"
                      :max-rows="10">
        </b-form-textarea>
      </b-form-group>
      <button class="btn btn-primary float-right ml-2" type="submit">Send</button>
    </b-form>
  </b-modal>
</template>

<script>
import { mapState } from 'vuex'
import VueMarkdown from 'vue-markdown'

export default {
  components: {
    VueMarkdown
  },
  data () {
    return {
      messages: [],
      form: {
        message: ''
      }
    }
  },
  computed: {
    ...mapState('context', [
      'profile'
    ])
  },
  created () {
    // Listen to answer changes from SignalR event
    this.$questionHub.$on('chat-message-received', this.onMessageReceived)
  },
  beforeDestroy () {
    // Make sure to cleanup SignalR event handlers when removing the component
    this.$questionHub.$off('chat-message-received', this.onMessageReceived)
  },
  methods: {
    onMessageReceived ({ username, text }) {
      this.messages = [...this.messages, { username, text }]
    },
    onSendMessage (evt) {
      this.$questionHub.sendMessage(this.form.message)
      this.form.message = ''
    },
    onHidden () {
      Object.assign(this.form, {
        message: ''
      })
    }
  }
}
</script>

 <style scoped>
 .messages-container{
   max-height: 450px;
   overflow-y: auto;
 }
 </style>
