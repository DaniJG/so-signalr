<template>
  <div class="hello">
    <h1>
      This totally looks like Stack Overflow
      <button v-b-modal.addQuestionModal class="btn btn-primary mt-2 float-right">
        <i class="fas fa-plus"/> Ask a question
      </button>
    </h1>
    <ul class="list-unstyled mt-4">
      <question-preview
        v-for="question in questions"
        :key="question.id"
        :question="question"
        class="mb-3" />
    </ul>
    <add-question-modal @question-added="onQuestionAdded"/>
  </div>
</template>

<script>
import QuestionPreview from '@/components/question-preview'
import AddQuestionModal from '@/components/add-question-modal'

export default {
  components: {
    QuestionPreview,
    AddQuestionModal
  },
  data () {
    return {
      questions: []
    }
  },
  created () {
    this.$http.get('/api/question').then(res => {
      this.questions = res.data
    })
  },
  methods: {
    onQuestionAdded (question) {
      console.log('dsfgfdg')
      this.questions = [question, ...this.questions]
    }
  }
}
</script>
