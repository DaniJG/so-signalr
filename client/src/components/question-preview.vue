<template>
  <li class="card container" @click="onOpenQuestion">
    <div class="card-body row">
      <question-score :question="question" class="col-1" />
      <div class="col-11">
        <h5 class="card-title">{{ question.title }}</h5>
        <p><vue-markdown :source="question.body" /></p>
        <a href="#" class="card-link">
          View question <span class="badge badge-success">{{ question.answerCount }}</span>
        </a>
      </div>
    </div>
  </li>
</template>

<script>
import VueMarkdown from 'vue-markdown'
import QuestionScore from '@/components/question-score'

export default {
  components: {
    VueMarkdown,
    QuestionScore
  },
  props: {
    question: {
      type: Object,
      required: true
    }
  },
  created () {
    // Listen to answer changes from SignalR event
    this.$questionHub.$on('answer-count-changed', this.onAnswerCountChanged)
  },
  beforeDestroy () {
    // Make sure to cleanup SignalR event handlers when removing the component
    this.$questionHub.$off('answer-count-changed', this.onAnswerCountChanged)
  },
  methods: {
    onOpenQuestion () {
      this.$router.push({ name: 'Question', params: { id: this.question.id } })
    },
    // This is called from the server through SignalR
    onAnswerCountChanged ({ questionId, answerCount }) {
      if (this.question.id !== questionId) return
      Object.assign(this.question, { answerCount })
    }
  }
}
</script>
