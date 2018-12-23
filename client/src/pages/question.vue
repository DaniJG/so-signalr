<template>
  <article class="container" v-if="question">
    <header class="row align-items-center">
      <question-score :question="question" class="col-1" />
      <h3 class="col-11">{{ question.title }}</h3>
    </header>
    <p class="row">
      <vue-markdown class="offset-1 col-11">{{ question.body }}</vue-markdown>
    </p>
    <ul class="list-unstyled row" v-if="hasAnswers">
      <li v-for="answer in question.answers" :key="answer.id" class="offset-1 col-11 border-top py-2">
        <vue-markdown>{{ answer.body }}</vue-markdown>
      </li>
    </ul>
    <footer>
      <button class="btn btn-link" @click="onReturnHome">Back to list</button>
      <button class="btn btn-primary float-right" @click="onAddAnswer">New Answer</button>
    </footer>
  </article>
</template>

<script>
import VueMarkdown from 'vue-markdown'
import QuestionScore from '@/components/question-score'

export default {
  components: {
    VueMarkdown,
    QuestionScore
  },
  data () {
    return {
      question: null,
      answers: [],
      questionid: this.$route.params.id
    }
  },
  computed: {
    hasAnswers () {
      return this.question.answers.length > 0
    }
  },
  created () {
    this.$http.get(`/api/question/${this.questionid}`).then(res => {
      this.question = res.data
    })
  },
  methods: {
    onReturnHome () {
      this.$router.push({name: 'Home'})
    },
    onAddAnswer () {

    }
  }
}
</script>
