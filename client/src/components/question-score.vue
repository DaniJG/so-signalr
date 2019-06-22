<template>
  <h3 class="text-center scoring">
    <button class="btn btn-link btn-lg p-0 d-block mx-auto" :disabled="!isAuthenticated" @click.stop="onUpvote"><i class="fas fa-sort-up" /></button>
    <span class="d-block mx-auto">{{ question.score }}</span>
    <button class="btn btn-link btn-lg p-0 d-block mx-auto" :disabled="!isAuthenticated" @click.stop="onDownvote"><i class="fas fa-sort-down" /></button>
  </h3>
</template>

<script>
import { mapGetters } from 'vuex'

export default {
  props: {
    question: {
      type: Object,
      required: true
    }
  },
  computed: {
    ...mapGetters('context', [
      'isAuthenticated'
    ])
  },
  created () {
    // Listen to score changes coming from SignalR events
    this.$questionHub.$on('score-changed', this.onScoreChanged)
  },
  beforeDestroy () {
    // Make sure to cleanup SignalR event handlers when removing the component
    this.$questionHub.$off('score-changed', this.onScoreChanged)
  },
  methods: {
    onUpvote () {
      this.$http.patch(`/api/question/${this.question.id}/upvote`).then(res => {
        Object.assign(this.question, res.data)
      })
    },
    onDownvote () {
      this.$http.patch(`/api/question/${this.question.id}/downvote`).then(res => {
        Object.assign(this.question, res.data)
      })
    },
    // This is called from the server through SignalR
    onScoreChanged ({ questionId, score }) {
      if (this.question.id !== questionId) return
      Object.assign(this.question, { score })
    }
  }
}
</script>

<style scoped>
.scoring .btn-link{
  line-height: 1;
}
</style>
