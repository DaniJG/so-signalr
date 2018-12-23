import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install (Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl(`${Vue.prototype.$http.defaults.baseURL}/question-hub`)
      .configureLogging(LogLevel.Information)
      .build()

    const questionHub = new Vue() // use new Vue instance as an event bus

    // Forward hub events through the event, so we can listen for them in the Vue components
    connection.on('SendQuestionScoreChange', question => {
      questionHub.$emit('question-score-changed', question)
    })

    // TODO: Add methods for components to send messages back to server

    // Add the hub to the Vue prototype, so every component can listen to events or send new events using this.$questionHub
    Vue.prototype.$questionHub = questionHub

    // You need to call connection.start() to establish the connection
    // the client wont handle reconnecting for you! Docs recommend listening onclose
    // and handling it there. This is the simplest of the strategies
    async function start () {
      try {
        await connection.start()
      } catch (err) {
        console.error('Failed to connect with hub', err)
        setTimeout(() => start(), 5000)
      }
    }
    connection.onclose(async () => {
      await start()
    })

    start()
  }
}
