<template>
  <li class="card container">
    <div class="card-body row">
      <h3 class="col-1 text-center scoring">
        <button class="btn btn-link btn-lg p-0 d-block mx-auto" @click="onUpvote"><i class="fas fa-sort-up" /></button>
        <span class="d-block mx-auto">{{ question.score }}</span>
        <button class="btn btn-link btn-lg p-0 d-block mx-auto" @click="onDownvote"><i class="fas fa-sort-down" /></button>
      </h3>
      <div class="col-11">
        <h5 class="card-title">{{ question.title }}</h5>
        <p><vue-markdown>{{ question.body }}</vue-markdown></p>
        <a href="#" class="card-link">View question</a>
      </div>
    </div>
  </li>
</template>

<script>
import VueMarkdown from 'vue-markdown'

export default {
  components: {
    VueMarkdown
  },
  props: {
    question: {
      type: Object,
      required: true
    }
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
