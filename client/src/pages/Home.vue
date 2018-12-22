<template>
  <div class="hello">
    <h1>
      This totally looks like Stack Overflow
      <button class="btn btn-primary mt-2 float-right"><i class="fas fa-plus"/> Ask a question</button>
    </h1>
    <question-list class="mt-4" />
  </div>
</template>

<script>
import QuestionList from '@/components/question-list'

export default {
  components: {
    QuestionList
  }
}
</script>
