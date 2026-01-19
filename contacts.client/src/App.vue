<template>
  <div class="app">
    <Feed />
  </div>
</template>

<script>
import Feed from "./components/Feed.vue";
export default {
  components: {
    Feed,
  },
};
</script>

<style scoped>
html,
body {
  width: 100%;
  height: 100%;
}
.app {
  background: linear-gradient(
    352deg,
    rgba(6, 92, 117, 1) 7%,
    rgba(39, 62, 189, 1) 42%,
    rgba(9, 19, 158, 1) 86%
  );

  min-height: 100vh;
  display: flex;
  justify-content: center;
  padding: 20px;
  box-sizing: border-box;
}
</style>
