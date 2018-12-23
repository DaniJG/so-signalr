<template>
  <h3 class="text-center scoring">
    <button class="btn btn-link btn-lg p-0 d-block mx-auto" @click.stop="onUpvote"><i class="fas fa-sort-up" /></button>
    <span class="d-block mx-auto">{{ question.score }}</span>
    <button class="btn btn-link btn-lg p-0 d-block mx-auto" @click.stop="onDownvote"><i class="fas fa-sort-down" /></button>
  </h3>
</template>

<script>
export default {
  props: {
    question: {
      type: Object,
      required: true
    }
  },
  created () {
    this.$questionHub.$on('score-changed', ({questionId, score}) => {
      if (this.question.id !== questionId) return
      Object.assign(this.question, { score })
    })
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
    }
  }
}
</script>

<style scoped>
.scoring .btn-link{
  line-height: 1;
}
</style>
