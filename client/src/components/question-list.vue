<template>
  <ul class="list-unstyled">
    <li v-for="question in questions" :key="question.id" class="card container">

      <div class="card-body row">
        <div class="col-1 text-center scoring">
          <button class="btn btn-link p-0 d-block mx-auto"><i class="fas fa-sort-up" /></button>
          <span class="d-block mx-auto">{{ question.score }}</span>
          <button class="btn btn-link p-0 d-block mx-auto"><i class="fas fa-sort-down" /></button>
        </div>
        <div class="col-11">
          <h5 class="card-title">{{ question.title }}</h5>
          <p><vue-markdown>{{ question.body }}</vue-markdown></p>
          <a href="#" class="card-link">View question</a>
        </div>
      </div>

    </li>
  </ul>
</template>

<script>
import VueMarkdown from 'vue-markdown'

export default {
  components: {
    VueMarkdown
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
  }
}
</script>

<style scoped>
.scoring .btn-link{
  line-height: 1;
}
</style>
